using WSMan.NET.SOAP;

namespace WSMan.NET.Client
{
    public interface IMessageSender
    {
        IncomingMessage SendAndGetResponse();
        IncomingMessage SendAndGetResponse(out IMessageBuilder nextMessageBuilder);
    }
}