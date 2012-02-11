using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Server;

namespace WSMan.NET.Management
{
    public class ManagementClient
    {
        private readonly SOAPClient _soapClient;

        public ManagementClient(string serverUrl)
        {
            _soapClient = new SOAPClient(serverUrl);
        }

        public T Get<T>(string resourceUri, string fragmentTransferExpression, params Selector[] selectors)
        {
            return Get<T>(resourceUri, fragmentTransferExpression, (IEnumerable<Selector>)selectors);
        }

        public T Get<T>(string resourceUri, string fragmentTransferExpression, IEnumerable<Selector> selectors)
        {
            return _soapClient.BuildMessage().Get<T>(resourceUri, fragmentTransferExpression, selectors);
        }
       
        public T Put<T>(string resourceUri, string fragmentTransferExpression, object payload, params Selector[] selectors)
        {
            return Put<T>(resourceUri, fragmentTransferExpression, payload, (IEnumerable<Selector>)selectors);
        }

        public T Put<T>(string resourceUri, string fragmentTransferExpression, object payload, IEnumerable<Selector> selectors)
        {
            return _soapClient.BuildMessage().Put<T>(resourceUri, fragmentTransferExpression, payload, selectors);
        }

        public EndpointReference Create(string resourceUri, object payload)
        {
            return _soapClient.BuildMessage().Create(resourceUri, payload);
        }

        public void Delete(string resourceUri, params Selector[] selectors)
        {
            Delete(resourceUri, (IEnumerable<Selector>)selectors);
        }

        public void Delete(string resourceUri, IEnumerable<Selector> selectors)
        {
            _soapClient.BuildMessage().Delete(resourceUri, selectors);
        }
    }
}