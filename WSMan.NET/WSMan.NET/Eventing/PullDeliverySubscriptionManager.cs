using System;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
    public class PullDeliverySubscriptionManager :
       ISubscriptionManager,
       IDisposable
    {
        public Subsciption Subscribe(Filter filter, IEnumerable<Selector> selectors, Expires expires, EndpointReference subscriptionManagerEndpointAddress)
        {
            var subscription = new PullSubscription(Guid.NewGuid().ToString(), _deliveryResourceUri, _eventType, filter, expires, selectors, this);
            _handler.Bind(subscription, subscriptionManagerEndpointAddress);
            _deliveryServer.AddSubscription(subscription);
            _subscriptions[subscription.Identifier] = subscription;
            subscriptionManagerEndpointAddress
                .AddProperty(new IdentifierHeader(subscription.Identifier), true)
                .AddProperty(new ResourceUriHeader(subscription.DeliveryResourceUri), true);
            return subscription;
        }

        public void Unsubscribe(Subsciption subscription)
        {
            var pullSubscription = (PullSubscription)subscription;
            _handler.Unbind(pullSubscription);
            _deliveryServer.RemoveSubscription(pullSubscription);
            _subscriptions.Remove(subscription.Identifier);
            subscription.Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            foreach (PullSubscription subscription in _subscriptions.Values)
            {
                _handler.Unbind(subscription);
                subscription.Dispose();
            }
            _disposed = true;
        }

        public PullDeliverySubscriptionManager(string deliveryResourceUri, EventingPullDeliveryServer deliveryServer, IEventingRequestHandler handler)
        {
            Type eventingRequestHandlerGenericInterface =
               handler.GetType().GetInterface(typeof(IEventingRequestHandler<>).Name);
            if (eventingRequestHandlerGenericInterface == null)
            {
                throw new InvalidOperationException("Eventing request handler must implement generic version of IEventingRequestHandler interface.");
            }
            _eventType = eventingRequestHandlerGenericInterface.GetGenericArguments()[0];
            _deliveryResourceUri = deliveryResourceUri;
            _handler = handler;
            _deliveryServer = deliveryServer;
        }

        private bool _disposed;
        private readonly Type _eventType;
        private readonly EventingPullDeliveryServer _deliveryServer;
        private readonly string _deliveryResourceUri;
        private readonly IEventingRequestHandler _handler;
        private readonly Dictionary<string, PullSubscription> _subscriptions = new Dictionary<string, PullSubscription>();
    }
}