using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.Server;

namespace WSMan.NET.Eventing.Client
{
    public class PullSubscriptionClientImpl<T> : IPullSubscriptionClient<T>
    {
        private bool _disposed;
        private readonly ISOAPClient _soapClient;
        private string _context;
        private readonly IEnumerable<IMessageHeaderWithMustUnderstandSpecification> _additionalHeaders;

        public PullSubscriptionClientImpl(ISOAPClient soapClient, string context, IEnumerable<IMessageHeaderWithMustUnderstandSpecification> additionalHeaders)
        {
            _soapClient = soapClient;
            _context = context;
            _additionalHeaders = additionalHeaders;
        }

        public IEnumerable<T> PullOnce()
        {
            var pullResponse = PullNextBatch(_context, 100);
            _context = pullResponse.EnumerationContext.Text;
            return pullResponse.Items == null 
                ? Enumerable.Empty<T>() 
                : pullResponse.Items.Select(x => x.DeserializeAs(typeof(T))).Cast<T>();
        }

        public IEnumerable<T> Pull()
        {
            bool endOfSequence = false;
            while (!endOfSequence)
            {
                PullResponse pullResponse = PullNextBatch(_context, 100);
                if (pullResponse.Items != null)
                {
                    foreach (var item in pullResponse.Items)
                    {
                        yield return (T)item.DeserializeAs(typeof(T));
                    }
                }
                endOfSequence = pullResponse.EndOfSequence != null;
                _context = pullResponse.EnumerationContext.Text;
            }
        }

        private PullResponse PullNextBatch(string context, int maxElements)
        {
            return _soapClient
                .BuildMessage()
                .AddHeaders(_additionalHeaders)
                .PullNextBatch(context, maxElements);
        }

        private void Unsubscribe()
        {
            _soapClient
                .BuildMessage()
                .AddHeaders(_additionalHeaders)
                .Unsubscribe(_context);
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