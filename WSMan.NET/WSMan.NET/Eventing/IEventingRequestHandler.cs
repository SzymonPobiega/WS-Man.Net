using System;
using System.Linq;
using System.ServiceModel;

namespace WSMan.NET.Eventing
{
   public interface IEventingRequestHandler<T> : IEventingRequestHandler
   {
   }

   public interface IEventingRequestHandler
   {
      void Bind(IEventingRequestHandlerContext context, EndpointAddressBuilder susbcriptionManagerEndpointAddress);
      void Unbind(IEventingRequestHandlerContext context);
   }
}