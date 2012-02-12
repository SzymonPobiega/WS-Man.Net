using WSMan.NET.SOAP;
using WSMan.NET.Transfer;

namespace WSMan.NET.Server
{
    public class MessageBuilder : IMessageBuilder
    {
        private readonly ISOAPClient _soapClient;
        private readonly OutgoingMessage _outgoingMessage = new OutgoingMessage();

        public MessageBuilder(ISOAPClient soapClient)
        {
            _soapClient = soapClient;
        }

        public IMessageBuilder AddHeader(IMessageHeader header, bool mustUnderstand)
        {
            _outgoingMessage.AddHeader(header, mustUnderstand);
            return this;
        }

        public IMessageSender AddBody(IBodyWriter bodyWriter)
        {
            _outgoingMessage.SetBody(bodyWriter);
            return this;
        }

        public IMessageSender AddBody(object serializableObject)
        {
            _outgoingMessage.SetBody(new SerializerBodyWriter(serializableObject));
            return this;
        }

        public IncomingMessage SendAndGetResponse()
        {
            return _soapClient.SendRequest(_outgoingMessage);
        }
    }
}