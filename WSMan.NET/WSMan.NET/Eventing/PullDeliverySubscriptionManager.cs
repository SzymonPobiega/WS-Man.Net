using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   public class PullDeliverySubscriptionManager : 
      IEnumerationRequestHandler,
      ISubscriptionManager,
      IDisposable
   {      
      public IEnumerable<object> Enumerate(string context, Filter filter)
      {
         PullSubscription subscription = null;
         return subscription.Enumerate(filter);
      }

      public Subsciption Subscribe()
      {         
         PullSubscription subscription = new PullSubscription(Guid.NewGuid().ToString(), this);
         _handler.Bind(subscription.Buffer);
         return subscription;
      }

      public void Unsubscribe(Subsciption subsciption)
      {
         PullSubscription pullSubscription = (PullSubscription) subsciption;
         _handler.Unbind(pullSubscription.Buffer);
         subsciption.Dispose();
      }

      public void Dispose()
      {
         if (_disposed)
         {
            return;
         }
         _disposed = true;
      }
      
      public PullDeliverySubscriptionManager(IEventingRequestHandler handler)
      {
         _handler = handler;
      }      

      private bool _disposed;
      private readonly IEventingRequestHandler _handler;      
   }
}