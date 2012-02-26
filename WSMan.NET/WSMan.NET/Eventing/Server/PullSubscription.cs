using System;
using System.Collections.Generic;
using System.Linq;

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
            Console.WriteLine("Event pushed.");
        }

        public IList<object> FetchNotifications(int maxElements, TimeSpan maxTime)
        {
            var notifications = _buffer.FetchNotifications(maxElements, maxTime).ToList();
            Console.WriteLine("Fetched "+notifications.Count());
            return notifications;
        }

        public void Dispose()
        {
            _eventSource.Pushed -= OnPushed;
        }
    }
}