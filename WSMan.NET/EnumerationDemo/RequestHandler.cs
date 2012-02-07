using System;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;
using WSMan.NET.SOAP;

namespace EnumerationTests
{
   public class RequestHandler : IEnumerationRequestHandler
   {
      public IEnumerable<object> Enumerate(IEnumerationContext context, IncomingMessage requestMessage, OutgoingMessage responseMessage)
      {
         Console.WriteLine("Server: Returning item");
         yield return new EndpointReference("http://tempuri-1.org");
         Console.WriteLine("Server: Returning item");
         yield return new EndpointReference("http://tempuri-2.org");
         Console.WriteLine("Server: Returning item");
         yield return new EndpointReference("http://tempuri-3.org");         
      }

      public int EstimateRemainingItemsCount(IEnumerationContext context, IncomingMessage requestMessage, OutgoingMessage responseMessage)
      {
         return 3;
      }
   }
}