using System;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Enumeration;

namespace EnumerationTests
{
   public class RequestHandler : IEnumerationRequestHandler
   {
      public IEnumerable<object> Enumerate(string context, Filter filter)
      {
         Console.WriteLine("Server: Returning item");
         yield return new EndpointAddress("http://tempuri-1.org");
         Console.WriteLine("Server: Returning item");
         yield return new EndpointAddress("http://tempuri-2.org");
         Console.WriteLine("Server: Returning item");
         yield return new EndpointAddress("http://tempuri-3.org");         
      }
   }
}