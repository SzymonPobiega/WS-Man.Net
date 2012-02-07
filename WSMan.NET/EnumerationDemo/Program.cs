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
              .Bind(new Uri("http://tempuri.org"), FilterMap.DefaultDialect, typeof (JmxNotificationFilter), new RequestHandler());

         using (new HttpListenerTransferEndpoint("http://localhost:12345/", enumerationServer))
         {
             Console.WriteLine("Press any key to exit.");
             Console.ReadKey();
         }
      }
   }   
}
