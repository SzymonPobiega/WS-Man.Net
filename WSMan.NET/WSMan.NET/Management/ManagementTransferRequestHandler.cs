using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Transfer;

namespace WSMan.NET.Management
{
    public class ManagementTransferRequestHandler : ITransferRequestHandler
    {
        public object HandleGet()
        {
            return GetHandler().HandleGet(GetFragmentExpression(), GetSelectors());
        }

        public object HandlePut(ExtractBodyDelegate extractBodyCallback)
        {
            return GetHandler().HandlePut(GetFragmentExpression(), GetSelectors(), extractBodyCallback);
        }

        public EndpointAddress HandleCreate(ExtractBodyDelegate extractBodyCallback)
        {
            return GetHandler().HandleCreate(extractBodyCallback);
        }

        public void HandlerDelete()
        {
            GetHandler().HandlerDelete(GetSelectors());
        }

        public void Bind(Uri resourceUri, IManagementRequestHandler handler)
        {
            _handlers[resourceUri.ToString()] = handler;
        }

        private static string GetFragmentExpression()
        {
            var fragmentTransferHeader = OperationContextProxy.Current.FindHeader<FragmentTransferHeader>();
            string fragmentExpression = null;
            if (fragmentTransferHeader != null)
            {
                OperationContextProxy.Current.AddHeader(fragmentTransferHeader);
                fragmentExpression = fragmentTransferHeader.Expression;
            }
            return fragmentExpression;
        }

        private IManagementRequestHandler GetHandler()
        {
            ResourceUriHeader resourceUriHeader = OperationContextProxy.Current.FindHeader<ResourceUriHeader>();

            //TODO: Fault
            return _handlers[resourceUriHeader.ResourceUri];
        }


        private static List<Selector> GetSelectors()
        {
            SelectorSetHeader selectorSetHeader = OperationContextProxy.Current.FindHeader<SelectorSetHeader>();

            List<Selector> selectors = selectorSetHeader != null
               ? selectorSetHeader.Selectors
               : new List<Selector>();
            return selectors;
        }

        private readonly Dictionary<string, IManagementRequestHandler> _handlers = new Dictionary<string, IManagementRequestHandler>();
    }
}