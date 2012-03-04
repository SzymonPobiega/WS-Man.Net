using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Management
{
    public class ManagementTransferRequestHandler : ITransferRequestHandler
    {
        public object HandleGet(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            var fragmentExpression = GetFragmentExpression(incomingHeaders, outgoingHeaders);
            var selectors = GetSelectors(incomingHeaders);
            var handler = GetHandler(incomingHeaders);

            return handler.HandleGet(fragmentExpression, selectors);
        }

        public object HandlePut(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback)
        {
            var fragmentExpression = GetFragmentExpression(incomingHeaders, outgoingHeaders);
            var selectors = GetSelectors(incomingHeaders);
            var handler = GetHandler(incomingHeaders);

            return handler.HandlePut(fragmentExpression, selectors, extractBodyCallback);
        }

        public EndpointReference HandleCreate(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback)
        {
            var handler = GetHandler(incomingHeaders);

            return handler.HandleCreate(extractBodyCallback);
        }

        public void HandlerDelete(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            var handler = GetHandler(incomingHeaders);

            handler.HandlerDelete(GetSelectors(incomingHeaders));
        }

        public void Bind(string resourceUri, IManagementRequestHandler handler)
        {
            _handlers[resourceUri] = handler;
        }

        private static string GetFragmentExpression(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            var fragmentTransferHeader = incomingHeaders.GetHeader<FragmentTransferHeader>();
            string fragmentExpression = null;
            if (fragmentTransferHeader != null)
            {
                outgoingHeaders.AddHeader(fragmentTransferHeader, true);
                fragmentExpression = fragmentTransferHeader.Expression;
            }
            return fragmentExpression;
        }

        private IManagementRequestHandler GetHandler(IIncomingHeaders incomingHeaders)
        {
            var resourceUriHeader = incomingHeaders.GetHeader<ResourceUriHeader>();

            //TODO: Fault
            return _handlers[resourceUriHeader.ResourceUri];
        }


        private static IEnumerable<Selector> GetSelectors(IIncomingHeaders incomingHeaders)
        {
            var selectorSetHeader = incomingHeaders.GetHeader<SelectorSetHeader>();

            List<Selector> selectors = selectorSetHeader != null
               ? selectorSetHeader.Selectors
               : new List<Selector>();
            return selectors;
        }

        private readonly Dictionary<string, IManagementRequestHandler> _handlers = new Dictionary<string, IManagementRequestHandler>();
    }
}