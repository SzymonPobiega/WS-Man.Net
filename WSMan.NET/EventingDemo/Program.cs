using System;
using WSMan.NET.Enumeration;
using WSMan.NET.Eventing.Client;
using WSMan.NET.Eventing.Server;
using WSMan.NET.Management;
using WSMan.NET.Server;

namespace EventingDemo
{
    class Program
    {
        private const string ResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";

        static void Main(string[] args)
        {            
            var eventingServer = new EventingServer(new RequestHandler());
            var pullDeliveryServer = eventingServer.EnablePullDelivery();
            eventingServer.Bind(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

            Console.WriteLine("WARNING! TimedOut exception which will cause your Visual Studio to break into");
            Console.WriteLine("debugging mode are part of WS-Eventing protocol and should be skipped");
            Console.WriteLine("with F5 while debugging.");
            Console.WriteLine();

            var client = new EventingClient("http://localhost:12345/Eventing");
            client.BindFilterDialect(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

            using (new HttpListenerTransferEndpoint("http://localhost:12345/",
                eventingServer, pullDeliveryServer))
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
