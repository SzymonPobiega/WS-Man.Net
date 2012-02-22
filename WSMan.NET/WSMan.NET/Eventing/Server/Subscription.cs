using System;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.SOAP;

namespace WSMan.NET.Eventing.Server
{
    public class Subscription : ISubscription
    {
        private bool _disposed;
        private DateTime _expirationDate;        
        private readonly IDisposable _handlerSubscription;

        public Subscription(Expires expires, IEventingRequestHandler requestHandler, object filterInstance, EndpointReference subscriptionManagerReference, IIncomingHeaders incomingHeaders)
        {
            _handlerSubscription = requestHandler.Subscribe(this, filterInstance, subscriptionManagerReference, incomingHeaders);
            Renew(expires);
        }

        public void Push(object evnt)
        {
            if (Pushed != null)
            {
                Pushed(this, new EventPushedEventArgs(evnt));
            }
        }

        public event EventHandler<EventPushedEventArgs> Pushed;
        public void Renew(Expires expires)
        {
            if (expires.Value is DateTime)
            {
                _expirationDate = (DateTime)expires.Value;
            }
            else
            {
                _expirationDate = DateTime.Now + (TimeSpan)expires.Value;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;                
            }
            _handlerSubscription.Dispose();
            _disposed = true;
        }
    }
}