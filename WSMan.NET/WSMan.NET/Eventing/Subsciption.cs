using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public abstract class Subsciption : IDisposable
   {
      public string Identifier
      {
         get { return _identifier; }
      }

      public void Unsubscribe()
      {
         _manager.Unsubscribe(this);
      }

      protected Subsciption(string identifier, ISubscriptionManager manager)
      {
         _identifier = identifier;
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
      private readonly ISubscriptionManager _manager;
   }
}