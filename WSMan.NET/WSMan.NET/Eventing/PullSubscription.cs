using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   public class PullSubscription : Subsciption
   {
      public PullSubscription(string identifier, PullDeliverySubscriptionManager manager) 
         : base(identifier, manager)
      {
      }

      public IEnumerable<object> Enumerate(Filter filter)
      {
         if (!_buffer.IsEmpty)
         {
            return _buffer.FetchNotifications();
         }
         else
         {
            throw new NotImplementedException();
         }
      }

      public EventBuffer Buffer
      {
         get { return _buffer; }
      }      
      
      private readonly EventBuffer _buffer = new EventBuffer();      
   }
}