using System;
using WSMan.NET.Addressing;
using WSMan.NET.Management;

namespace ManagementDemo
{
    public class Client
    {
        public static void PerformTest()
        {
            var client = new ManagementClient("http://localhost:12345/Management");

            Console.WriteLine("Client: requesting fragment {0} of resource {1} with selector [{2}/{3}]", Program.ResourceUri, "a", "name", "value");
            Console.WriteLine();
            var o = client.Get<XmlFragment<SampleData>>(Program.ResourceUri, "a", new[] { new Selector("name", "value"), });

            Console.WriteLine("Client: putting fragment {0} of resource {1}: {2}", Program.ResourceUri, "fragment", o.Value);
            Console.WriteLine();
            var a = client.Put<SampleData>(Program.ResourceUri, "fragment", o.Value);

            Console.WriteLine("Client: deleting resource {0} with selector [{1}/{2}]", Program.ResourceUri, "name", "http://tempuri.org");
            Console.WriteLine();
            client.Delete(Program.ResourceUri, new[] { new Selector("name", new EndpointReference("http://tempuri.org")) });
        }
    }
}