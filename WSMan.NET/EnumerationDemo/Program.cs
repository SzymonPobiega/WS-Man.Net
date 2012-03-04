using System;
using WSMan.NET.Enumeration;
using WSMan.NET.Server;

namespace EnumerationTests
{
   class Program
   {
      static void Main(string[] args)
      {
          var enumerationServer = new EnumerationServer()
              .Bind(FilterMap.DefaultDialect, typeof (JmxNotificationFilter), new RequestHandler());

         using (new HttpListenerTransferEndpoint("http://localhost:12345/", enumerationServer))
         {
             Client.PerformTest();
             Console.WriteLine("Press any key to exit.");
             Console.ReadKey();
         }
      }
   }
}
