using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace WSMan.NET.WCF
{
   public class DummyOperationContext : IOperationContext
   {
      private readonly List<IExtension<OperationContext>> _extensions = new List<IExtension<OperationContext>>();
      private readonly List<object> _headers = new List<object>();

      public T FindExtension<T>() where T : IExtension<OperationContext>
      {
         return (T)_extensions.Find(x => x.GetType() == typeof (T));
      }

      public void AddExtension<T>(T item) where T : IExtension<OperationContext>
      {
         _extensions.Add(item);
      }

      public T FindHeader<T>()
      {
         return (T)_headers.Find(x => x.GetType() == typeof(T));
      }

      public void AddHeader<T>(T item)
      {
         _headers.Add(item);
      }

      public Uri LocalAddress
      {
         get; set;
      }
   }


}