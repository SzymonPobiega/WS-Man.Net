using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public interface IMessageBuilder : IMessageSender
    {
        IMessageBuilder AddHeader(IMessageHeader header, bool mustUnderstand);
        IMessageSender AddBody(IBodyWriter bodyWriter);
        IMessageSender AddBody(object serializableObject);
    }
}