using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
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

        public IEnumerable<EndpointReference> EnumerateEPR(string resourceUri, Filter filter, int maxElements, params Selector[] selectors)
        {
            return EnumerateEPR(resourceUri, filter, maxElements, (IEnumerable<Selector>)selectors);
        }

        public int EstimateCount(string resourceUri, Filter filter, params Selector[] selectors)
        {
            return EstimateCount(resourceUri, filter, (IEnumerable<Selector>)selectors);
        }

        public int EstimateCount(string resourceUri, Filter filter, IEnumerable<Selector> selectors)
        {
            return _soapClient
                .BuildMessage()
                .EstimateEnumerationCount(resourceUri, filter, selectors);
        }

        public IEnumerable<EndpointReference> EnumerateEPR(string resourceUri, Filter filter, int maxElements, IEnumerable<Selector> selectors)
        {
            var response = _soapClient
                .BuildMessage()
                .StartEnumeration(resourceUri, selectors, filter, EnumerationMode.EnumerateEPR, _optimize);            

            if (response.Items != null)
            {
                foreach (var item in response.Items)
                {
                    yield return item.EPRValue;
                }
            }
            var context = response.EnumerationContext;
            var endOfSequence = response.EndOfSequence != null;
            while (!endOfSequence)
            {
                var pullResponse = _soapClient
                    .BuildMessage()
                    .PullNextBatch(context, resourceUri, maxElements, selectors);

                foreach (var item in pullResponse.Items)
                {
                    yield return item.EPRValue;
                }
                endOfSequence = pullResponse.EndOfSequence != null;
                context = pullResponse.EnumerationContext;
            }
        }
    }
}