using System;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing.Server
{
    public interface ISubscription : IEventSink, IEventSource, IDisposable
    {
        void Renew(Expires expires);        
    }
}