using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration.Faults;
using WSMan.NET.Management;
using WSMan.NET.Server;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Enumeration
{
    public class EnumerationServer : AddressingBasedRequestHandler
    {
        private readonly Dictionary<EnumerationContextKey, EnumerationState> _activeEnumerations = new Dictionary<EnumerationContextKey, EnumerationState>();
        private readonly Dictionary<HandlerMapKey, IEnumerationRequestHandler> _handlerMap = new Dictionary<HandlerMapKey, IEnumerationRequestHandler>();
        private readonly FilterMap _filterMap = new FilterMap();

        public EnumerationServer Bind(Uri resourceUri, string dialect, Type filterType, IEnumerationRequestHandler handler)
        {
            _filterMap.Bind(dialect, filterType);
            _handlerMap[new HandlerMapKey(resourceUri.ToString(), dialect)] = handler;
            return this;
        }
        
        protected override OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
        {
            switch (actionHeader.Action)
            {
                case Constants.EnumerateAction:
                    return Enumerate(request);
                case Constants.PullAction:
                    return Pull(request);
                default:
                    return null;
            }
        }

        private OutgoingMessage Enumerate(IncomingMessage requestMessage)
        {
            var request = requestMessage.GetPayload<EnumerateRequest>();
            var contextKey = EnumerationContextKey.Unique();
            var selectorSetHeader = requestMessage.GetHeader<SelectorSetHeader>();
            var selectors = selectorSetHeader != null ? selectorSetHeader.Selectors : Enumerable.Empty<Selector>();
            var filter = CreateFilterInstance(request.Filter);
            var context = new EnumerationContext(contextKey.Text, filter, selectors);

            if (IsCountRequest(requestMessage))
            {
                return HandleCountEnumerate(requestMessage, contextKey, request, context);
            }
            if (request.OptimizeEnumeration != null)
            {
                return HandleOptimizedEnumerate(requestMessage, contextKey, request, context);
            }
            return HandleNormalEnumerate(requestMessage, contextKey, request, context);
        }

        private object CreateFilterInstance(Filter filter)
        {
            var filterType = _filterMap.GetFilterType(filter.Dialect);
            if (filterType == null)
            {
                throw new NotSupportedDialectFaultException();
            }
            return filter.DeserializeAs(filterType);
        }

        private static bool IsCountRequest(IncomingMessage incomingMessage)
        {
            return incomingMessage.GetHeader<RequestTotalItemsCountEstimateHeader>() != null;
        }

        private OutgoingMessage HandleNormalEnumerate(IncomingMessage requestMessage, EnumerationContextKey contextKey, EnumerateRequest request, EnumerationContext context)
        {
            var responseMessage = CreateEnumerateResponse();
            IEnumerator<object> enumerator = GetHandler(request.Filter, requestMessage)
                .Enumerate(context, requestMessage, responseMessage)
                .GetEnumerator();
            _activeEnumerations[contextKey] = new EnumerationState(enumerator, request.EnumerationMode);

            responseMessage.SetBody(
                new SerializerBodyWriter(new EnumerateResponse
                                             {
                                                 EnumerationContext = contextKey,
                                                 Expires = request.Expires
                                             }));
            return responseMessage;
        }

        

        private OutgoingMessage HandleCountEnumerate(IncomingMessage requestMessage, EnumerationContextKey contextKey, EnumerateRequest request, EnumerationContext context)
        {
            var responseMessage = CreateEnumerateResponse();
            int count = GetHandler(request.Filter, requestMessage)
                .EstimateRemainingItemsCount(context, requestMessage, responseMessage);

            responseMessage.AddHeader(new TotalItemsCountEstimateHeader(count), false);
            responseMessage.SetBody(new SerializerBodyWriter(new EnumerateResponse
                                                          {
                                                              EnumerationContext = contextKey
                                                          }));
            return responseMessage;
        }

        private OutgoingMessage HandleOptimizedEnumerate(IncomingMessage requestMessage, EnumerationContextKey contextKey, EnumerateRequest request, EnumerationContext context)
        {
            var responseMessage = CreateEnumerateResponse();
            var maxElements = request.MaxElements != null
                                 ? request.MaxElements.Value
                                 : 1;

            if (request.EnumerationMode == EnumerationMode.EnumerateEPR)
            {
                var enumerator = GetHandler(request.Filter, requestMessage)
                    .Enumerate(context, requestMessage,responseMessage)
                    .GetEnumerator();

                bool endOfSequence;
                var items = new EnumerationItemList(PullItems(maxElements, request.EnumerationMode, enumerator, out endOfSequence));
                if (!endOfSequence)
                {
                    _activeEnumerations[contextKey] = new EnumerationState(enumerator, request.EnumerationMode);
                }
                responseMessage.SetBody(new SerializerBodyWriter(
                                     new EnumerateResponse
                                         {
                                             Items = items,
                                             EndOfSequence = endOfSequence ? new EndOfSequence() : null,
                                             EnumerationContext = endOfSequence ? null : contextKey
                                         }));
                return responseMessage;
            }
            throw new NotSupportedException();
        }

        private OutgoingMessage Pull(IncomingMessage requestMessage)
        {
            var response = CreatePullResponse();
            var request = requestMessage.GetPayload<PullRequest>();
            EnumerationState holder;
            if (!_activeEnumerations.TryGetValue(request.EnumerationContext, out holder))
            {
                throw new InvalidEnumerationContextFaultException();
            }

            var maxElements = request.MaxElements != null
                                  ? request.MaxElements.Value
                                  : 1;

            bool endOfSequence;
            var items =
                new EnumerationItemList(PullItems(maxElements, holder.Mode, holder.Enumerator, out endOfSequence));
            if (endOfSequence)
            {
                _activeEnumerations.Remove(request.EnumerationContext);
            }
            response.SetBody(
                new SerializerBodyWriter(new PullResponse
                                             {
                                                 Items = items,
                                                 EndOfSequence = endOfSequence ? new EndOfSequence() : null,
                                                 EnumerationContext = endOfSequence ? null : request.EnumerationContext
                                             }));
            return response;
        }        

        private static IEnumerable<EnumerationItem> PullItems(int maximum, EnumerationMode mode, IEnumerator<object> enumerator, out bool endOfSequence)
        {
            int i = 0;
            var result = new List<EnumerationItem>();
            bool moveNext = false;
            while (i < maximum && (moveNext = enumerator.MoveNext()))
            {
                if (mode == EnumerationMode.EnumerateEPR)
                {
                    result.Add(new EnumerationItem((EndpointReference)enumerator.Current));
                }
                else
                {
                    result.Add(new EnumerationItem(
                                  new EndpointReference("http://tempuri.org"),
                                  enumerator.Current));
                }
                i++;
            }
            endOfSequence = !moveNext || i < maximum;
            return result;
        }

        private IEnumerationRequestHandler GetHandler(Filter filter, IncomingMessage requestMessage)
        {
            //TODO: Add fault if not found
            var resourceUriHeader = requestMessage.GetHeader<ResourceUriHeader>();
            var dialect = (filter != null && filter.Dialect != null)
               ? filter.Dialect
               : FilterMap.DefaultDialect;

            return _handlerMap[new HandlerMapKey(resourceUriHeader.ResourceUri, dialect)];
        }

        private static OutgoingMessage CreatePullResponse()
        {
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.PullResponseAction), false);
        }

        private static OutgoingMessage CreateEnumerateResponse()
        {
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.EnumerateResponseAction), false);
        }

        
    }
}