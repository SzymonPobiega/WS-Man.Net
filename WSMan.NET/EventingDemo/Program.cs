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
            eventingServer.BindWithPullDelivery(ResourceUri, FilterMap.DefaultDialect, typeof(JmxNotificationFilter), new RequestHandler(), ResourceDeliveryUri);

            Console.WriteLine("WARNING! TimedOut exception which will cause your Visual Studio to break into");
            Console.WriteLine("debugging mode are part of WS-Eventing protocol and should be skipped");
            Console.WriteLine("with F5 while debugging.");
            Console.WriteLine();

            var client = new EventingClient("http://localhost:12345/Eventing");
            client.BindFilterDialect(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

            using (new HttpListenerTransferEndpoint("http://localhost:12345/", eventingServer))
            {
                using (client.SubscribeUsingPullDelivery<EventData>(
                  x => Console.WriteLine(x.Value),
                  true,
                  ResourceUri,
                  ResourceUri,
                  new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()),
                  new Selector("name", "value")))
                {
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                }
            }
        }
    }
}
