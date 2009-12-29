using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public interface ISubscriptionManager
   {
      Subsciption Subscribe();
      void Unsubscribe(Subsciption subsciption);
   }
}