using System;

namespace WSMan.NET.Eventing.Server
{
    public interface IEventSource
    {
        event EventHandler<EventPushedEventArgs> Pushed;
    }
}