using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Fault;

namespace WSMan.NET
{
   public static class FaultFactoryExtensions
   {
      public static bool IsA(this FaultException exception, FaultFactory factory)
      {
         return factory.Check(exception);
      }      
   }
}