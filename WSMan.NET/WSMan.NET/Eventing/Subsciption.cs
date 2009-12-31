using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public abstract class Subsciption : IEventingRequestHandlerContext, IDisposable
   {
      public string Identifier
      {
         get { return _identifier; }
      }

      public Filter Filter
      {
         get { return _filter; }
      }

      public IEnumerable<Selector> Selectors
      {
         get { return _selectors; }
      }

      public string DeliveryResourceUri
      {
         get { return _deliveryResourceUri; }
      }

      public abstract void Push(object @event);
      
      public void Unsubscribe()
      {
         _manager.Unsubscribe(this);
      }

      protected Subsciption(string identifier, string deliveryResourceUri, Filter filter, IEnumerable<Selector> selectors, ISubscriptionManager manager)
      {
         _identifier = identifier;
         _deliveryResourceUri = deliveryResourceUri;
         _filter = filter;
         _selectors = selectors;
         _manager = manager;
      }

      protected virtual void Dispose(bool disposing)
      {
         if (_disposed)
         {
            return;
         }
         if (disposing)
         {               
         }
         _disposed = true;
      }

      public void Dispose()
      {
         Dispose(true);
      }

      private bool _disposed;
      private readonly string _identifier;
      private readonly string _deliveryResourceUri;
      private readonly Filter _filter;
      private readonly IEnumerable<Selector> _selectors;
      private readonly ISubscriptionManager _manager;      
   }
}