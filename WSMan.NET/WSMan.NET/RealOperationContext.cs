using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET
{
   public class RealOperationContext : IOperationContext
   {
      private readonly OperationContext _realContext;

      public RealOperationContext(OperationContext realContext)
      {
         _realContext = realContext;
      }

      public T FindExtension<T>() where T : IExtension<OperationContext>
      {
         return _realContext.Extensions.Find<T>();
      }

      public void AddExtension<T>(T item) where T : IExtension<OperationContext>
      {
         _realContext.Extensions.Add(item);
      }

      public T FindHeader<T>()
      {
         MethodInfo readMethod = typeof (T).GetMethod("ReadFrom", BindingFlags.Public | BindingFlags.Static, null,
                              new[] {typeof (MessageHeaders)}, null);
         return (T)readMethod.Invoke(null, new object[] { _realContext.IncomingMessageHeaders});
      }

      public void AddHeader<T>(T item)
      {
         _realContext.OutgoingMessageHeaders.Add((MessageHeader)(object)item);
      }

      public Uri LocalAddress
      {
         get { return _realContext.IncomingMessageHeaders.To; }
      }
   }
}