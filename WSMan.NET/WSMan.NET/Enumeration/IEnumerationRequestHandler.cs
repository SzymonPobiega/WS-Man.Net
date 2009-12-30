using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSMan.NET.Enumeration
{
   public interface IEnumerationContext
   {
      string Context { get; }
      Filter Filter { get; }
   }

   public interface IEnumerationRequestHandler
   {
      IEnumerable<object> Enumerate(IEnumerationContext context);
   }
}
