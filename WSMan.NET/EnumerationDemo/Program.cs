using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using WSMan.NET.Enumeration;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace EnumerationTests
{
   class Program
   {
      const int BatchSize = 100;
      const bool Optimize = true;

      static void Main(string[] args)
      {
         EnumerationServer server = new EnumerationServer().Bind(
            new Uri("http://tempuri.org"), 
            FilterMap.DefaultDialect, 
            typeof(JmxNotificationFilter),
            new RequestHandler());         

         ServiceHost sh = new ServiceHost(server);

         Binding binding = new WSHttpBinding(SecurityMode.None);
         sh.AddServiceEndpoint(typeof(IWSEnumerationContract), binding, "http://localhost/Contract");

         ServiceBehaviorAttribute behavior = sh.Description.Behaviors.Find<ServiceBehaviorAttribute>();
         behavior.ConcurrencyMode = ConcurrencyMode.Multiple;
         behavior.InstanceContextMode = InstanceContextMode.Single;
         behavior.IncludeExceptionDetailInFaults = true;         

         sh.Open();         

         Console.WriteLine("Client: Enumerating with batch size {0}, {1}optimizing enumeration", BatchSize, !Optimize ? "not ": "");
         Console.WriteLine();
         EnumerationClient client = new EnumerationClient(Optimize, new Uri("http://localhost/Contract"), binding);
         client.BindFilterDialect(FilterMap.DefaultDialect, typeof(JmxNotificationFilter));

         int countEstimate = client.EstimateCount(new Uri("http://tempuri.org"),
                                                  new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()));         
         Console.WriteLine("Client: Total items estimate: {0}", countEstimate);

         foreach (EndpointAddress item in client.EnumerateEPR(new Uri("http://tempuri.org"), new Filter(FilterMap.DefaultDialect, new JmxNotificationFilter()), BatchSize))
         {
            Console.WriteLine("Client: Got item {0}", item);
         }

         Console.WriteLine("Press any key to exit.");
         Console.ReadKey();
         sh.Close();
      }
   }   
}
