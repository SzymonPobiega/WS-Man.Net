﻿using System;

namespace WSMan.NET.Eventing.Server
{
    public class SubscribedEventArgs : EventArgs
    {
        private readonly IEventSource _eventSource;
        private readonly string _identifier;

        public SubscribedEventArgs(IEventSource eventSource, string identifier)
        {
            _eventSource = eventSource;
            _identifier = identifier;
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public IEventSource EventSource
        {
            get { return _eventSource; }
        }
    }
}