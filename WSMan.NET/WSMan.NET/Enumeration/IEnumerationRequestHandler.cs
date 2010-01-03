using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSMan.NET.Management;

namespace WSMan.NET.Enumeration
{
   public interface IEnumerationContext
   {
      string Context { get; }
      Filter Filter { get; }
      IEnumerable<Selector> Selectors { get; }
   }

   public interface IEnumerationRequestHandler
   {
      IEnumerable<object> Enumerate(IEnumerationContext context);
      int EstimateRemainingItemsCount(IEnumerationContext context);
   }
}
