using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Fault
{
   public class ManagementFaultFactory : FaultFactory
   {
      public ManagementFaultFactory(string reason, string code) 
         : base(reason, code, Management.Const.Namespace, Management.Const.FaultAction)
      {
      }      
   }
}