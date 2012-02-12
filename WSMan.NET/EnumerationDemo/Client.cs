using System;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;

namespace EnumerationTests
{
    public class Client
    {
        const int BatchSize = 100;
        const bool Optimize = true;

        public static void PerformTest()
        {
            var client = new EnumerationClient(Optimize, "http://localhost:12345/Enumeration");
            client.BindFilterDialect(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

            int countEstimate = client.EstimateCount("http://tempuri.org", new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()));
            Console.WriteLine("Client: Total items estimate: {0}", countEstimate);

            foreach (EndpointReference item in client.EnumerateEPR("http://tempuri.org", new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()), BatchSize))
            {
                Console.WriteLine("Client: Got item {0}", item);
            }
        }
    }
}