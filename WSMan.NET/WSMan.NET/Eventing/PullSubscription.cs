using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   public class PullSubscription : Subsciption
   {
      public PullSubscription(string identifier, PullDeliverySubscriptionManager manager) 
         : base(identifier, manager)
      {
      }
      
      public EventBuffer Buffer
      {
         get { return _buffer; }
      }      
      
      private readonly EventBuffer _buffer = new EventBuffer();      
   }
}