using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Faults
{
   public class InvalidEnumerationContextException : FaultException
   {
      public InvalidEnumerationContextException()
         : base("The supplied enumeration context is invalid.", 
                FaultCode.CreateReceiverFaultCode(Const.Namespace, "InvalidEnumerationContext"),
                Const.FaultAction)
      {         
      }
   }
}