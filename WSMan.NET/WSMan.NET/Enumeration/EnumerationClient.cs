using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Client;
using WSMan.NET.Management;
using WSMan.NET.Server;

namespace WSMan.NET.Enumeration
{
    public class EnumerationClient
    {
        private readonly ISOAPClient _soapClient;
        private readonly bool _optimize;
        private readonly FilterMap _filterMap = new FilterMap();

        public EnumerationClient(bool optimize, ISOAPClient soapClient)
        {
            _optimize = optimize;
            _soapClient = soapClient;
        }

        public EnumerationClient(bool optimize, string serverUrl)
            : this(optimize, new SOAPClient(serverUrl))
        {
        }

        public EnumerationClient BindFilterDialect(string dialect, Type implementationType)
        {
            _filterMap.Bind(dialect, implementationType);
            return this;
        }

        public int EstimateCount(string resourceUri, Filter filter, params Selector[] selectors)
        {
            return EstimateCount(resourceUri, filter, (IEnumerable<Selector>)selectors);
        }

        public int EstimateCount(string resourceUri, Filter filter, IEnumerable<Selector> selectors)
        {
            return _soapClient
                .BuildMessage()
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .EstimateEnumerationCount(filter);
        }

        public IEnumerable<EndpointReference> EnumerateEPR(string resourceUri, Filter filter, int maxElements, params Selector[] selectors)
        {
            return EnumerateEPR(resourceUri, filter, maxElements, (IEnumerable<Selector>)selectors);
        }

        public IEnumerable<EndpointReference> EnumerateEPR(string resourceUri, Filter filter, int maxElements, IEnumerable<Selector> selectors)
        {
            return _soapClient
                .BuildMessage()
                .WithResourceUri(resourceUri)
                .WithSelectors(selectors)
                .EnumerateEPR(filter, maxElements, _optimize);
        }
    }
}