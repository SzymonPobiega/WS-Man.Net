using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public interface ISOAPClient
    {
        IMessageBuilder BuildMessage();
        IncomingMessage SendRequest(OutgoingMessage requestMessage);
    }
}