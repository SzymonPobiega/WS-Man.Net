using System;
using System.Linq;

namespace WSMan.NET.Eventing
{
   public interface IEventingRequestHandler
   {
      void Bind(IEventingRequestHandlerContext context);
      void Unbind(IEventingRequestHandlerContext context);
   }
}