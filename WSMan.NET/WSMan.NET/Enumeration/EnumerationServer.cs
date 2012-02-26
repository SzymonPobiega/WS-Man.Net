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
        private readonly Dictionary<HandlerMapKey, IEnumerationRequestHandler> _handlerMap = new Dictionary<HandlerMapKey, IEnumerationRequestHandler>();
        private readonly FilterMap _filterMap = new FilterMap();
        private readonly PullServer _pullServer = new PullServer();

        public PullServer PullServer
        {
            get { return _pullServer; }
        }

        public EnumerationServer Bind(string resourceUri, string dialect, Type filterType, IEnumerationRequestHandler handler)
        {
            _filterMap.Bind(dialect, filterType);
            _handlerMap[new HandlerMapKey(resourceUri, dialect)] = handler;
            return this;
        }

        protected override OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
        {
            return actionHeader.Action == Constants.EnumerateAction
                ? Enumerate(request)
                : PullServer.Handle(request);
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
            var enumerator = GetEnumerator(request, requestMessage, context, responseMessage);

            OnEnumerationStarted(context.Context, enumerator, request.EnumerationMode);

            responseMessage.SetBody(
                new SerializerBodyWriter(new EnumerateResponse
                                             {
                                                 EnumerationContext = contextKey,
                                                 Expires = request.Expires
                                             }));
            return responseMessage;
        }

        private IEnumerator<object> GetEnumerator(EnumerateRequest request, IncomingMessage requestMessage, EnumerationContext context, OutgoingMessage responseMessage)
        {
            return GetHandler(request.Filter, requestMessage)
                .Enumerate(context, requestMessage, responseMessage)
                .GetEnumerator();
        }

        private void OnEnumerationStarted(string context, IEnumerator<object> enumerator, EnumerationMode enumerationMode)
        {
            _pullServer.RegisterPullHandler(context, new EnumerationPullHandler(enumerator, enumerationMode));
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
            var maxElements = CalculateMaxElements(request.MaxElements);
            if (request.EnumerationMode == EnumerationMode.EnumerateEPR)
            {
                var enumerator = GetEnumerator(request, requestMessage, context, responseMessage);
                bool endOfSequence;
                var items = enumerator.Take(maxElements, out endOfSequence);
                if (!endOfSequence)
                {
                    OnEnumerationStarted(context.Context, enumerator, request.EnumerationMode);
                }
                responseMessage.SetBody(new SerializerBodyWriter(
                                     new EnumerateResponse
                                         {
                                             Items = new EnumerationItemList(items, request.EnumerationMode),
                                             EndOfSequence = endOfSequence ? new EndOfSequence() : null,
                                             EnumerationContext = endOfSequence ? null : contextKey
                                         }));
                return responseMessage;
            }
            throw new NotSupportedException();
        }                     

        private IEnumerationRequestHandler GetHandler(Filter filter, IncomingMessage requestMessage)
        {
            var resourceUriHeader = requestMessage.GetHeader<ResourceUriHeader>();
            var dialect = (filter != null && filter.Dialect != null)
               ? filter.Dialect
               : FilterMap.DefaultDialect;

            var key = new HandlerMapKey(resourceUriHeader.ResourceUri, dialect);
            IEnumerationRequestHandler supportedDialectHandler;
            if (_handlerMap.TryGetValue(key, out supportedDialectHandler))
            {
                return supportedDialectHandler;
            }
            throw new NotSupportedDialectFaultException();
        }        

        private static OutgoingMessage CreateEnumerateResponse()
        {
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.EnumerateResponseAction), false);
        }

        private static int CalculateMaxElements(MaxElements maxElementsElement)
        {
            return maxElementsElement != null
                       ? maxElementsElement.Value
                       : 1;
        }

    }
}