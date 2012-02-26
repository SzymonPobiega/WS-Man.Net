using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using WSMan.NET.Enumeration;
using WSMan.NET.Management.Faults;

namespace WSMan.NET.Eventing.Server
{
    public class EventingPullHandler : IPullHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (EventingPullHandler));
        private bool _disposed;
        private readonly PullSubscription _subscription;

        public EventingPullHandler(IEventSource eventSource)
        {
            _subscription = new PullSubscription(eventSource);
        }

        public PullResult Pull(int? maxElements, TimeSpan? maxTime, string context)
        {
            Log.InfoFormat("Pulling events from subscription {0}", context);
            var items = _subscription.FetchEvents(maxElements ?? 1, maxTime ?? TimeSpan.FromSeconds(5));
            ReturnTimedOutFaultIfNoPendingEvents(items, context);
            Log.InfoFormat("Returning {0} events from subscription {0}", items.Count);
            return new PullResult(items, EnumerationMode.EnumerateObjectAndEPR, false);
        }
        
        private static void ReturnTimedOutFaultIfNoPendingEvents(IEnumerable<object> items, string context)
        {
            if (!items.Any())
            {
                Log.InfoFormat("No events pending for subscription {0}. Returning TimedOut fault.", context);
                throw new TimedOutFaultException();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _subscription.Dispose();
            _disposed = true;
        }

    }
}