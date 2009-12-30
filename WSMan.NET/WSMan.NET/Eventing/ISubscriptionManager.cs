using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   public interface ISubscriptionManager
   {
      Subsciption Subscribe(Filter filter);
      void Unsubscribe(Subsciption subsciption);
   }
}