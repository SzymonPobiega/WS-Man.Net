using System;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Eventing.Server
{
   public interface IEventingRequestHandler<T> : IEventingRequestHandler
   {
   }

   public interface IEventingRequestHandler
   {
      IDisposable Subscribe( 
          IEventSink eventSink, 
          object filterInstance,
          EndpointReference subscriptionManagerReference, 
          IIncomingHeaders headers);
   }
}