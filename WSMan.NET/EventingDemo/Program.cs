using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Management;

namespace EventingDemo
{
   [XmlRoot(ElementName = "NotificationFilter", Namespace = "http://jsr262.dev.java.net")]
   public class JmxNotificationFilter
   {

   }

   public class RequestHandler : IEventingRequestHandler
   {
      private Timer _timer;
      private readonly List<IEventingRequestHandlerContext> _subscribers = new List<IEventingRequestHandlerContext>();

      public RequestHandler()
      {
         _timer = new Timer(Publish, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));         
      }

      public void Bind(IEventingRequestHandlerContext context)
      {             
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
            subscriber.Push(new EndpointAddress("http://tempuri.org"));
         }
      }
   }

   class Program
   {
      static void Main(string[] args)
      {
         EventingServer eventingServer = new EventingServer();
         eventingServer.BindWithPullDelivery(FilterMap.DefaultDialect, typeof(JmxNotificationFilter ), new RequestHandler());
         
         ServiceHost sh = new ServiceHost(eventingServer);

         Binding binding = new BasicHttpBinding();
         sh.AddServiceEndpoint(typeof(IWSEventingPullDeliveryContract), binding, "http://simon-hp:80/Contract");
         sh.AddServiceEndpoint(typeof(IWSEventingContract), binding, "http://simon-hp:80/Contract");

         ServiceBehaviorAttribute behavior = sh.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
         behavior.InstanceContextMode = InstanceContextMode.Single;
         behavior.IncludeExceptionDetailInFaults = true;

         sh.Open();

         EventingClient client = new EventingClient(new Uri("http://localhost:8888/Contract"), new BasicHttpBinding());
         client.BindFilterDialect(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

         IPullSubscriptionClient subscriptionClient =
            client.SubscribeWithPullDelivery(new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()));

         foreach (EndpointAddress item in subscriptionClient.Pull())
         {
            Console.WriteLine(item);
         }

         Console.ReadKey();
         sh.Close();
      }
   }
}
