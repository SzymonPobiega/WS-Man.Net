using System;
using System.ServiceModel;

namespace WSMan.NET.WCF
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