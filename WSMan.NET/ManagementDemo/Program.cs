using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using WSMan.NET;
using WSMan.NET.Management;
using WSMan.NET.Transfer;
using Const=WSMan.NET.Management.Const;

namespace ManagementDemo
{
   class Program
   {
      private const string ResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";

      static void Main(string[] args)
      {         
         ManagementTransferRequestHandler handler = new ManagementTransferRequestHandler();
         handler.Bind(new Uri(ResourceUri), new WsManHandler());
         ServiceHost sh = new ServiceHost(new TransferServer(handler));

         Binding binding = new BasicHttpBindingWithAddressing();
         sh.AddServiceEndpoint(typeof(IWSTransferContract), binding, "http://localhost/Contract");

         var behavior = sh.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
         behavior.InstanceContextMode = InstanceContextMode.Single;

         sh.Open();

         var cf = new ChannelFactory<IWSTransferContract>(new BasicHttpBindingWithAddressing());
         var client = new ManagementClient(new Uri("http://localhost/Contract"), cf, MessageVersion.Soap12WSAddressingAugust2004);

         //Console.WriteLine("Client: requesting fragment {0} of resource {1} with selector [{2}/{3}]", ResourceUri, "a", "name", "value");
         //Console.WriteLine();
         //var o = client.Get<XmlFragment<SampleData>>(ResourceUri, "a", new[] { new Selector("name", "value"), });

         //Console.WriteLine("Client: putting fragment {0} of resource {1}: {2}", ResourceUri, "fragment", o.Value);
         //Console.WriteLine();
         //var a = client.Put<SampleData>(ResourceUri, "fragment", o.Value);

         //Console.WriteLine("Client: deleting resource {0} with selector [{1}/{2}]", ResourceUri, "name", "http://tempuri.org");
         //Console.WriteLine();
         //client.Delete(ResourceUri, new[] { new Selector("name", new EndpointAddress("http://tempuri.org"))});

         Console.WriteLine("Press any key to exit.");
         Console.ReadKey();

         cf.Close();
         sh.Close();
      }
   }
}