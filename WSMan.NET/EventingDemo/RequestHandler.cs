using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using WSMan.NET.Eventing;

namespace EventingDemo
{
   public class RequestHandler : IEventingRequestHandler<EventData>
   {
      private Timer _timer;
      private readonly List<IEventingRequestHandlerContext> _subscribers = new List<IEventingRequestHandlerContext>();

      public RequestHandler()
      {
         _timer = new Timer(Publish, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));         
      }

      public void Bind(IEventingRequestHandlerContext context)
      {             
         Console.WriteLine("Got subscription with filter {0}.", context.Filter);
         _subscribers.Add(context);
      }

      public void Unbind(IEventingRequestHandlerContext context)
      {
         _subscribers.Remove(context);
      }

      private void Publish(object state)
      {
         foreach (IEventingRequestHandlerContext subscriber in _subscribers)
         {
            subscriber.Push(new EventData {Value = "Some data"});
         }
      }
   }
}