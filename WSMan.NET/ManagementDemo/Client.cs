using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WSMan.NET.WCF;

namespace ManagementDemo
{
    public class Client
    {
        public static void PerformTest()
        {
            var cf = new ChannelFactory<IWSTransferContract>(new BasicHttpBindingWithAddressing());
            var client = new ManagementClient(new Uri("http://localhost:12345/Management"), cf,
                                              MessageVersion.Soap12WSAddressingAugust2004);

            Console.WriteLine("Client: requesting fragment {0} of resource {1} with selector [{2}/{3}]", Program.ResourceUri, "a", "name", "value");
            Console.WriteLine();
            var o = client.Get<XmlFragment<SampleData>>(Program.ResourceUri, "a", new[] { new Selector("name", "value"), });

            Console.WriteLine("Client: putting fragment {0} of resource {1}: {2}", Program.ResourceUri, "fragment", o.Value);
            Console.WriteLine();
            var a = client.Put<SampleData>(Program.ResourceUri, "fragment", o.Value);

            Console.WriteLine("Client: deleting resource {0} with selector [{1}/{2}]", Program.ResourceUri, "name", "http://tempuri.org");
            Console.WriteLine();
            client.Delete(Program.ResourceUri, new[] { new Selector("name", new EndpointAddress("http://tempuri.org")) });
        }
    }
}