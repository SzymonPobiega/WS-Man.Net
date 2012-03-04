using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public interface IMessageSender
    {
        IncomingMessage SendAndGetResponse();
        IncomingMessage SendAndGetResponse(out IMessageBuilder nextMessageBuilder);
    }
}