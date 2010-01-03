using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET
{
   public interface IOperationContext
   {
      Uri LocalAddress { get; }

      T FindExtension<T>() where T : IExtension<OperationContext>;
      void AddExtension<T>(T item) where T : IExtension<OperationContext>;

      T FindHeader<T>();
      void AddHeader<T>(T item);
   }
}