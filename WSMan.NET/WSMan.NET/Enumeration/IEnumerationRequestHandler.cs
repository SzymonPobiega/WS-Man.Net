using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSMan.NET.Enumeration
{
   public interface IEnumerationRequestHandler
   {
      IEnumerable<object> Enumerate(string context, Filter filter);
   }
}
