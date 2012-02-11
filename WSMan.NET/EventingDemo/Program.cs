using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSMan.NET;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing;
using WSMan.NET.Management;
using WSMan.NET.Server;

namespace EventingDemo
{   
   class Program
   {
      private const string ResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";
      private const string ResourceDeliveryUri = "http://jsr262.dev.java.net/MBeanNotificationSubscriptionManager";

      static void Main(string[] args)
      {
         var eventingServer = new EventingServer();
         eventingServer.BindWithPullDelivery(new Uri(ResourceUri), FilterMap.DefaultDialect, typeof(JmxNotificationFilter), new RequestHandler(), new Uri(ResourceDeliveryUri));

         using (new HttpListenerTransferEndpoint("http://localhost:12345/", eventingServer))
         {
             Console.WriteLine("Press any key to exit.");
             Console.ReadKey();
         }
      }
   }
}
