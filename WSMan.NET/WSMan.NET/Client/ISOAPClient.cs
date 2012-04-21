using WSMan.NET.SOAP;

namespace WSMan.NET.Client
{
    public interface ISOAPClient
    {
        IMessageBuilder BuildMessage();
        IncomingMessage SendRequest(OutgoingMessage requestMessage);
    }
}