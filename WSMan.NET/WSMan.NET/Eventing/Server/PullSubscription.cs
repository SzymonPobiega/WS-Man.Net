using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace WSMan.NET.Eventing.Server
{
    public class PullSubscription : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PullSubscription));

        private readonly EventBuffer _buffer = new EventBuffer();
        private readonly IEventSource _eventSource;

        public PullSubscription(IEventSource eventSource)
        {
            _eventSource = eventSource;
            _eventSource.Pushed += OnPushed;
        }

        private void OnPushed(object s, EventPushedEventArgs e)
        {
            _buffer.Push(e.Event);
            Log.InfoFormat("Event pushed: {0}.", e.Event);
        }

        public IList<object> FetchEvents(int maxElements, TimeSpan maxTime)
        {
            var events = _buffer.FetchNotifications(maxElements, maxTime).ToList();
            Log.InfoFormat("Fetched {0} events.", events.Count);
            return events;
        }

        public void Dispose()
        {            
            _eventSource.Pushed -= OnPushed;
        }
    }
}