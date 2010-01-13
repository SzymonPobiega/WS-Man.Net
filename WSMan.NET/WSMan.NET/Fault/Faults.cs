using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Fault;

namespace WSMan.NET
{
   public class Faults
   {
      public static readonly FaultFactory TimedOut = new ManagementFaultFactory(
         "The operation has timed out.", "TimedOut");

      public static readonly FaultFactory EndpointUnavailable = new AddressingFaultFactory(
         "The specified endpoint is currently unavailable.", "EndpointUnavailable");

      public static readonly FaultFactory InvalidEnumerationContext = new EnumerationFaultFactory(
         "The supplied enumeration context is invalid.", "InvalidEnumerationContext");
   }
}