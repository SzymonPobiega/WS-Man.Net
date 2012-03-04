using System.Collections.Generic;
using System.Linq;
using log4net;
using WSMan.NET.Addressing;
using WSMan.NET.Addressing.Faults;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Management
{
    public class ManagementTransferRequestHandler : ITransferRequestHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ManagementTransferRequestHandler));
        private readonly Dictionary<string, IManagementRequestHandler> _handlers = new Dictionary<string, IManagementRequestHandler>();

        public ManagementTransferRequestHandler Bind(string resourceUri, IManagementRequestHandler handler)
        {
            Log.InfoFormat("Binding '{0}' resource to {1}.", resourceUri, handler);
            _handlers[resourceUri] = handler;
            return this;
        }

        public object HandleGet(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            Log.InfoFormat("Handling Get request");
            var fragmentExpression = GetFragmentExpression(incomingHeaders, outgoingHeaders);
            var selectors = GetSelectors(incomingHeaders);
            var handler = GetHandler(incomingHeaders);

            var result = handler.HandleGet(fragmentExpression, selectors);
            Log.InfoFormat("Get request handled successfully");
            return result;
        }

        public object HandlePut(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback)
        {
            Log.InfoFormat("Handling Put request");
            var fragmentExpression = GetFragmentExpression(incomingHeaders, outgoingHeaders);
            var selectors = GetSelectors(incomingHeaders);
            var handler = GetHandler(incomingHeaders);

            var result = handler.HandlePut(fragmentExpression, selectors, extractBodyCallback);
            Log.InfoFormat("Put request handled successfully");
            return result;
        }

        public EndpointReference HandleCreate(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback)
        {
            Log.InfoFormat("Handling Create request");
            var handler = GetHandler(incomingHeaders);
            var result = handler.HandleCreate(extractBodyCallback);
            Log.InfoFormat("Create request handled successfully");
            return result;
        }

        public void HandlerDelete(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            Log.InfoFormat("Handling Delete request");
            var handler = GetHandler(incomingHeaders);
            handler.HandlerDelete(GetSelectors(incomingHeaders));
            Log.InfoFormat("Delete request handled successfully");
        }

        private static string GetFragmentExpression(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            var fragmentTransferHeader = incomingHeaders.GetHeader<FragmentTransferHeader>();
            string fragmentExpression = null;
            if (fragmentTransferHeader != null)
            {
                Log.InfoFormat("Requesting fragment '{0}'", fragmentTransferHeader.Expression);
                outgoingHeaders.AddHeader(fragmentTransferHeader, true);
                fragmentExpression = fragmentTransferHeader.Expression;
            }
            else
            {
                Log.Debug("Not a fragment request.");
            }
            return fragmentExpression;
        }

        private IManagementRequestHandler GetHandler(IIncomingHeaders incomingHeaders)
        {
            var resourceUriHeader = incomingHeaders.GetHeader<ResourceUriHeader>();
            if (resourceUriHeader == null)
            {
                Log.ErrorFormat("Required ResourceURI header not found.");
                throw new DestinationUnreachableFaultException();
            }
            var resourceUri = resourceUriHeader.ResourceUri;
            IManagementRequestHandler handler;
            if (_handlers.TryGetValue(resourceUri, out handler))
            {
                Log.InfoFormat("Using {0} to handle resource '{1}'", handler, resourceUri);
                return handler;
            }
            Log.ErrorFormat("Handler for resource '{0}' not found.", resourceUri);
            throw new DestinationUnreachableFaultException();
        }


        private static IEnumerable<Selector> GetSelectors(IIncomingHeaders incomingHeaders)
        {
            var selectorSetHeader = incomingHeaders.GetHeader<SelectorSetHeader>();

            if (selectorSetHeader != null)
            {
                Log.Info("Requesting selectors "+string.Join(", ", selectorSetHeader.Selectors.Select(x => x.ToString())));
                return selectorSetHeader.Selectors;
            }
            return Enumerable.Empty<Selector>();
        }
    }
}