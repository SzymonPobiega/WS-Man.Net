using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public interface IMessageSender
    {
        IncomingMessage SendAndGetResponse();
    }
}