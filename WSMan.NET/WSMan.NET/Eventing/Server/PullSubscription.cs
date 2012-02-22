using System;
using System.Collections.Generic;

namespace WSMan.NET.Eventing.Server
{
    public class PullSubscription : IDisposable
    {
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
        }

        public IEnumerable<object> FetchNotifications(int maxElements, TimeSpan maxTime)
        {
            return _buffer.FetchNotifications(maxElements, maxTime);
        }

        public void Dispose()
        {
            _eventSource.Pushed -= OnPushed;
        }
    }
}