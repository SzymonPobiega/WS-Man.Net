using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public interface ISubscriptionManager
   {
      Subsciption Subscribe(Filter filter, IEnumerable<Selector> selectors, Expires expires, EndpointReference subscriptionManagerEndpointAddress);
      void Unsubscribe(Subsciption subscription);
   }
}