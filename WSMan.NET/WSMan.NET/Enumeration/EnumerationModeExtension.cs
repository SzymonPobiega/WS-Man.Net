using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace WSMan.NET.Enumeration
{
   public class EnumerationModeExtension : IExtension<OperationContext>
   {
      private readonly EnumerationMode _mode;
      private readonly Type _enumeratedType;

      private EnumerationModeExtension(EnumerationMode mode, Type enumeratedType)
      {
         _mode = mode;
         _enumeratedType = enumeratedType;
      }

      public static void Activate(EnumerationMode mode, Type enumeratedType)
      {
         OperationContext.Current.Extensions.Add(new EnumerationModeExtension(mode, enumeratedType));
      }

      public static EnumerationMode CurrentEnumerationMode
      {
         get { return OperationContext.Current.Extensions.Find<EnumerationModeExtension>()._mode; }
      }

      public static Type CurrentEnumeratedType
      {
         get { return OperationContext.Current.Extensions.Find<EnumerationModeExtension>()._enumeratedType; }
      }

      public void Attach(OperationContext owner)
      {
      }

      public void Detach(OperationContext owner)
      {
      }
   }
}