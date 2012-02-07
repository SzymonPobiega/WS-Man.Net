using System.Collections.Generic;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration
{
    public interface IEnumerationRequestHandler
   {
      IEnumerable<object> Enumerate(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage);
      int EstimateRemainingItemsCount(IEnumerationContext context, IncomingMessage incomingMessage, OutgoingMessage outgoingMessage);
   }
}
