using System;

namespace WSMan.NET.Eventing.Server
{
    public class EventPushedEventArgs : EventArgs
    {
        private readonly object _evnt;

        public EventPushedEventArgs(object evnt)
        {
            _evnt = evnt;
        }

        public object Event
        {
            get { return _evnt; }
        }
    }
}