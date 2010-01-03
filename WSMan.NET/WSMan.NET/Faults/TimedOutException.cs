using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WSMan.NET.Faults
{
   public class TimedOutException : FaultException
   {
      public TimedOutException()
         : base("The operation has timed out.",
                FaultCode.CreateReceiverFaultCode(Management.Const.Namespace, "TimedOut"),
                Management.Const.FaultAction)
      {         
      }
   }
}