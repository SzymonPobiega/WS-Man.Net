using System;
using System.Collections.Generic;
using System.Linq;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration.Faults;
using WSMan.NET.Server;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Enumeration
{
    public class PullServer : AddressingBasedRequestHandler
    {
        private readonly Dictionary<string, IPullHandler> _handlers = new Dictionary<string,IPullHandler>();

        protected override OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
        {
            return actionHeader.Action == Constants.PullAction 
                ? Pull(request) 
                : null;
        }

        public void RegisterPullHandler(string context, IPullHandler handler)
        {
            lock (_handlers)
            {
                _handlers.Add(context, handler);
            }
        }

        public void UnregisterPullHandler(string context)
        {
            lock (_handlers)
            {
                _handlers.Remove(context);
            }
        }

        private OutgoingMessage Pull(IncomingMessage requestMessage)
        {
            var response = CreatePullResponse();
            var request = requestMessage.GetPayload<PullRequest>();
            IPullHandler handler;
            var context = request.EnumerationContext.Text;
            lock (_handlers)
            {
                if (!_handlers.TryGetValue(context, out handler))
                {
                    throw new InvalidEnumerationContextFaultException();
                }
            }
            var maxElements = GetMaxElements(request);
            var maxTime = GetMaxTime(request);
            var pullResult = handler.Pull(maxElements, maxTime, context);
            var items = new EnumerationItemList(pullResult.Items, pullResult.EnumerationMode);
            if (pullResult.EndOfSequence)
            {
                handler.Dispose();
                lock (_handlers)
                {
                    _handlers.Remove(context);
                }
            }
            response.SetBody(
                new SerializerBodyWriter(new PullResponse
                                             {
                                                 Items = items,
                                                 EndOfSequence = pullResult.EndOfSequence ? new EndOfSequence() : null,
                                                 EnumerationContext = pullResult.EndOfSequence ? null : request.EnumerationContext
                                             }));
            return response;
        }

        private static IEnumerable<EnumerationItem> EncapsulateItems(IEnumerable<object> enumerable, EnumerationMode enumerationMode)
        {
            return enumerationMode == EnumerationMode.EnumerateEPR ?
                EncapsulateEPRs(enumerable) 
                : EncapsulateObjects(enumerable);
        }

        private static IEnumerable<EnumerationItem> EncapsulateObjects(IEnumerable<object> enumerable)
        {
            return enumerable.Select(x => new EnumerationItem(new EndpointReference("http://tempuri.org"), x));
        }

        private static IEnumerable<EnumerationItem> EncapsulateEPRs(IEnumerable<object> enumerable)
        {
            return enumerable.Select(x => new EnumerationItem((EndpointReference) x));
        }

        private static int? GetMaxElements(PullRequest request)
        {
            return request.MaxElements != null 
                       ? request.MaxElements.Value 
                       : (int?)null;
        }

        private static TimeSpan? GetMaxTime(PullRequest request)
        {
            return request.MaxTime != null
                       ? request.MaxTime.Value
                       : (TimeSpan?)null;
        }

        private static OutgoingMessage CreatePullResponse()
        {
            return new OutgoingMessage()
                .AddHeader(new ActionHeader(Constants.PullResponseAction), false);
        }        
    }
}