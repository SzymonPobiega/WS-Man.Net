using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET
{
   public class AddressingVersionExtension : IExtension<OperationContext>
   {
      private readonly AddressingVersion _version;

      private AddressingVersionExtension(AddressingVersion version)
      {
         _version = version;
      }

      public static void Activate(AddressingVersion version)
      {
         OperationContextProxy.Current.AddExtension(new AddressingVersionExtension(version));
      }

      public static AddressingVersion CurrentVersion
      {
         get { return OperationContextProxy.Current.FindExtension<AddressingVersionExtension>()._version; }
      }

      public void Attach(OperationContext owner)
      {
         
      }

      public void Detach(OperationContext owner)
      {         
      }
   }
}