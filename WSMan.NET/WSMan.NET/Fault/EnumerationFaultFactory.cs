using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Fault
{
   public class EnumerationFaultFactory : FaultFactory
   {
      public EnumerationFaultFactory(string reason, string code)
         : base(reason, code, Enumeration.Const.Namespace, Enumeration.Const.FaultAction)
      {
      }
   }
}