using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;
using WSMan.NET.Management.Faults;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Eventing.Client
{
    public class PullSubscriptionClientImpl<T> : IPullSubscriptionClient<T>
    {
        private bool _disposed;
        private readonly ISOAPClient _soapClient;
        private readonly string _resourceUri;
        private EnumerationContextKey _context;

        public PullSubscriptionClientImpl(ISOAPClient soapClient, EnumerationContextKey context, string resourceUri)
        {
            _soapClient = soapClient;
            _resourceUri = resourceUri;
            _context = context;
        }

        public IEnumerable<T> PullOnce()
        {
            var pullResponse = PullNextBatch(_context, 100, Enumerable.Empty<Selector>());
            _context = pullResponse.EnumerationContext;
            return pullResponse.Items == null 
                ? Enumerable.Empty<T>() 
                : pullResponse.Items.Select(x => x.DeserializeAs(typeof(T))).Cast<T>();
        }

        public IEnumerable<T> Pull()
        {
            bool endOfSequence = false;
            while (!endOfSequence)
            {
                PullResponse pullResponse = PullNextBatch(_context, 100, Enumerable.Empty<Selector>());
                if (pullResponse.Items != null)
                {
                    foreach (EnumerationItem item in pullResponse.Items)
                    {
                        yield return (T)item.DeserializeAs(typeof(T));
                    }
                }
                endOfSequence = pullResponse.EndOfSequence != null;
                _context = pullResponse.EnumerationContext;
            }
        }

        private PullResponse PullNextBatch(EnumerationContextKey context, int maxElements, IEnumerable<Selector> selectors)
        {
            var requestMessage = _soapClient.BuildMessage()
                .WithAction(Enumeration.Constants.PullAction)
                .WithResourceUri(_resourceUri)
                .WithSelectors(selectors)
                .AddBody(new PullRequest
                             {
                                 MaxTime = new MaxTime(TimeSpan.FromSeconds(1)),
                                 EnumerationContext = context,
                                 MaxElements = new MaxElements(maxElements)
                             });
            try
            {
                var responseMessage = requestMessage.SendAndGetResponse();
                return responseMessage.GetPayload<PullResponse>();
            }
            catch (FaultException ex)
            {
                if (new TimedOutFaultException().Equals(ex))
                {
                    return new PullResponse
                               {
                                   EnumerationContext = context
                               };
                }
                throw;
            }
        }

        private void Unsubscribe()
        {
            _soapClient.BuildMessage()
                .WithAction(Constants.UnsubscribeAction)
                .WithResourceUri(_resourceUri)
                .AddHeader(new IdentifierHeader(_context.Text), true)
                .SendAndGetResponse();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            Unsubscribe();
            _disposed = true;
        }
    }
}