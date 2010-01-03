using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public class PullSubscription : Subsciption
   {
      public PullSubscription(string identifier, string deliveryUri, Type eventType, Filter filter, Expires expires, IEnumerable<Selector> selectors, ISubscriptionManager manager) 
         : base(identifier, deliveryUri, eventType, filter, expires, selectors, manager)
      {
      }

      public override void Push(object @event)
      {
         _buffer.Push(@event);
      }
      
      public EventBuffer Buffer
      {
         get { return _buffer; }
      }      
      
      private readonly EventBuffer _buffer = new EventBuffer();      
   }
}