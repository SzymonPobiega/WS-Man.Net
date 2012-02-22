using System;
using System.Collections.Generic;
using System.Threading;
using WSMan.NET.Addressing;
using WSMan.NET.Eventing.Server;
using WSMan.NET.SOAP;

namespace EventingDemo
{
   public class RequestHandler : IEventingRequestHandler<EventData>
   {
      private readonly Timer _timer;
      private readonly List<IEventSink> _subscribers = new List<IEventSink>();

      public RequestHandler()
      {
         _timer = new Timer(Publish, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));         
      }

      private void Publish(object state)
      {
         foreach (var subscriber in _subscribers)
         {
            subscriber.Push(new EventData {Value = "Some data"});
         }
      }

       public IDisposable Subscribe(IEventSink eventSink, object filterInstance, EndpointReference subscriptionManagerReference, IIncomingHeaders headers)
       {
           Console.WriteLine("Got subscription with filter {0}", filterInstance);
           _subscribers.Add(eventSink);
           return new SubscriptionLifetime(() => _subscribers.Remove(eventSink));
       }

       private class SubscriptionLifetime : IDisposable
       {
           private readonly Action _unsubscribeAction;

           public SubscriptionLifetime(Action unsubscribeAction)
           {
               _unsubscribeAction = unsubscribeAction;
           }

           public void Dispose()
           {
               _unsubscribeAction();
           }
       }
   }
}