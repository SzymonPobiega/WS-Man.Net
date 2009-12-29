using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public interface IEventingRequestHandlerContext
   {
      void Push(object @event);
   }
}