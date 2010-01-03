using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public interface ISubscriptionManager
   {
      Subsciption Subscribe(Filter filter, IEnumerable<Selector> selectors, Expires expires, EndpointAddressBuilder susbcriptionManagerEndpointAddress);
      void Unsubscribe(Subsciption subsciption);
   }
}