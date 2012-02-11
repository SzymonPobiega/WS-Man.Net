using WSMan.NET.Addressing;

namespace WSMan.NET.Eventing
{
   public interface IEventingRequestHandler<T> : IEventingRequestHandler
   {
   }

   public interface IEventingRequestHandler
   {
      void Bind(IEventingRequestHandlerContext context, EndpointReference subscriptionManagerReference);
      void Unbind(IEventingRequestHandlerContext context);
   }
}