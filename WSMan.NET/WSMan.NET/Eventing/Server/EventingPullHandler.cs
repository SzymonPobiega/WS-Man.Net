using System;
using System.Collections.Generic;
using System.Linq;
using WSMan.NET.Enumeration;
using WSMan.NET.Management.Faults;

namespace WSMan.NET.Eventing.Server
{
    public class EventingPullHandler : IPullHandler
    {
        private readonly PullSubscription _subscription;

        public EventingPullHandler(IEventSource eventSource)
        {
            _subscription = new PullSubscription(eventSource);
        }

        public PullResult Pull(int? maxElements, TimeSpan? maxTime, string context)
        {
            var items = _subscription.FetchNotifications(maxElements ?? 1, maxTime ?? TimeSpan.FromSeconds(5));
            ReturnTimedOutFaultIfNoPendingEvents(items);
            Console.WriteLine("Returning pull result with "+items.Count()+" items");
            return new PullResult(items, EnumerationMode.EnumerateObjectAndEPR, false);
        }
        
        private static void ReturnTimedOutFaultIfNoPendingEvents(IEnumerable<object> items)
        {
            if (!items.Any())
            {
                throw new TimedOutFaultException();
            }
        }

        public void Dispose()
        {
            //NOOP
        }

    }
}