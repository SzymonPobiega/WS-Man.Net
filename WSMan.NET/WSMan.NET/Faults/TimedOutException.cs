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
                FaultCode.CreateReceiverFaultCode("TimedOut", Management.Const.Namespace),
                Management.Const.FaultAction)
      {         
      }      
   }   

   public static class TimedOutExceptionExtensions
   {
      public static bool IsTimedOut(this FaultException exception)
      {
         return exception.Code.IsReceiverFault &&
                exception.Code.SubCode != null &&
                exception.Code.SubCode.Name == "TimedOut" &&
                exception.Code.SubCode.Namespace == Management.Const.Namespace;
      }
   }
}