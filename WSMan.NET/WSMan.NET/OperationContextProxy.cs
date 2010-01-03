using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace WSMan.NET
{
   public class OperationContextProxy
   {
      private static readonly DummyOperationContext _dummy = new DummyOperationContext();

      public static DummyOperationContext Dummy { get { return _dummy; } }

      public static IOperationContext Current
      {         
         get
         {
            if (OperationContext.Current != null)
            {
               return new RealOperationContext(OperationContext.Current);
            }
            return _dummy;
         }
      }
   }
}