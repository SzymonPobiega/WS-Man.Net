using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public interface IEventingRequestHandlerContext
   {
      Filter Filter { get; }
      IEnumerable<Selector> Selectors { get; }
      void Push(object @event);
   }
}