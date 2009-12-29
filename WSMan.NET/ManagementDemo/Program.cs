﻿using System;
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
         ServiceHost sh = new ServiceHost(
            new TransferServer(
               new ManagementServer(new List<IManagementRequestHandler>{ new WsManHandler()}), MessageVersion.Soap11));

         Binding binding = new BasicHttpBinding();
         sh.AddServiceEndpoint(typeof(ITransferContract), binding, "http://localhost/Contract");

         ServiceBehaviorAttribute behavior = sh.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
         behavior.InstanceContextMode = InstanceContextMode.Single;

         sh.Open();

         ChannelFactory<ITransferContract> cf = new ChannelFactory<ITransferContract>(new BasicHttpBinding());
         ManagementClient client = new ManagementClient(new Uri("http://localhost/Contract"), cf, MessageVersion.Soap11);

         Console.WriteLine("Client: requesting fragment {0} of resource {1} with selector [{2}/{3}]", ResourceUri, "a", "name", "value");
         Console.WriteLine();
         XmlFragment<SampleData> o = client.Get<XmlFragment<SampleData>>(ResourceUri, "a", new[] { new Selector("name", "value"), });

         Console.WriteLine("Client: putting fragment {0} of resource {1}: {2}", ResourceUri, "fragment", o.Value);
         Console.WriteLine();
         SampleData a = client.Put<SampleData>(ResourceUri, "fragment", o.Value);

         Console.WriteLine("Client: deleting resource {0} with selector [{1}/{2}]", ResourceUri, "name", "http://tempuri.org");
         Console.WriteLine();
         client.Delete(ResourceUri, new[] { new Selector("name", new EndpointAddress("http://tempuri.org"))});

         Console.WriteLine("Press any key to exit.");
         Console.ReadKey();

         cf.Close();
         sh.Close();
      }
   }
}