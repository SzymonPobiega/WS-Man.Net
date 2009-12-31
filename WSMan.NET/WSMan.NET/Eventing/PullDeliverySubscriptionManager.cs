using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public class PullDeliverySubscriptionManager :       
      ISubscriptionManager,
      IDisposable
   {            
      public Subsciption Subscribe(Filter filter, IEnumerable<Selector> selectors)
      {         
         PullSubscription subscription = new PullSubscription(Guid.NewGuid().ToString(), _deliveryResourceUri, filter, selectors, this);
         _handler.Bind(subscription);         
         _deliveryServer.AddSubscription(subscription);         
         _subscriptions[subscription.Identifier] = subscription;
         return subscription;
      }

      public void Unsubscribe(Subsciption subsciption)
      {
         PullSubscription pullSubscription = (PullSubscription) subsciption;
         _handler.Unbind(pullSubscription);
         _deliveryServer.RemoveSubscription(pullSubscription);
         _subscriptions.Remove(subsciption.Identifier);
         subsciption.Dispose();
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
         _deliveryResourceUri = deliveryResourceUri;
         _handler = handler;
         _deliveryServer = deliveryServer;
      }

      private bool _disposed;
      private readonly EventingPullDeliveryServer _deliveryServer;
      private readonly string _deliveryResourceUri;
      private readonly IEventingRequestHandler _handler;      
      private readonly Dictionary<string, PullSubscription> _subscriptions = new Dictionary<string, PullSubscription>();
   }
}