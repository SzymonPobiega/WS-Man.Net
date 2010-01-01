using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSMan.NET;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Management;

namespace EventingDemo
{   
   class Program
   {
      private const string ResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";
      private const string ResourceDeliveryUri = "http://jsr262.dev.java.net/MBeanNotificationSubscriptionManager";

      static void Main(string[] args)
      {
         EventingServer eventingServer = new EventingServer();
         eventingServer.BindWithPullDelivery(new Uri(ResourceUri), new Uri(ResourceDeliveryUri),  new RequestHandler(), FilterMap.DefaultDialect, typeof(JmxNotificationFilter ));
         
         ServiceHost sh = new ServiceHost(eventingServer);

         Binding binding = new WSHttpBindingAugust2004(SecurityMode.None);
         sh.AddServiceEndpoint(typeof(IWSEventingPullDeliveryContract), binding, "http://simon-hp:80/Contract");
         sh.AddServiceEndpoint(typeof(IWSEventingContract), binding, "http://simon-hp:80/Contract");

         ServiceBehaviorAttribute behavior = sh.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
         behavior.InstanceContextMode = InstanceContextMode.Single;
         behavior.IncludeExceptionDetailInFaults = true;
         behavior.AddressFilterMode = AddressFilterMode.Any;

         sh.Open();

         EventingClient client = new EventingClient(new Uri("http://localhost:8888/Contract"), binding);
         client.BindFilterDialect(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

         IPullSubscriptionClient<EventData> subscriptionClient =
            client.SubscribeWithPullDelivery<EventData>(new Uri(ResourceUri), 
            new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()),
            new Selector("name", "value"));

         foreach (EventData item in subscriptionClient.Pull())
         {
            Console.WriteLine(item.Value);
         }

         Console.ReadKey();
         sh.Close();
      }
   }
}
