using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using WSMan.NET.Addressing;
using WSMan.NET.Client;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration
{
    public class TestingSOAPClient : ISOAPClient
    {
        private readonly IRequestHandler[] _handlers;

        public TestingSOAPClient(params IRequestHandler[] handlers)
        {
            _handlers = handlers;
        }

        public IMessageBuilder BuildMessage()
        {
            return new MessageBuilder(this);
        }

        public IncomingMessage SendRequest(OutgoingMessage requestMessage)
        {
            requestMessage.AddHeader(new ToHeader("http://example.com"), true);
            var serverReceivedMessage = Receive(requestMessage);

            var serverResponse = Handle(serverReceivedMessage);

            var clientReceivedMessage = Receive(serverResponse);
            return clientReceivedMessage;
        }

        private static IncomingMessage Receive(OutgoingMessage outgoingMessage)
        {
            var buffer = new StringBuilder();
            using (var writer = XmlWriter.Create(buffer))
            {
                outgoingMessage.Write(writer);
                writer.Flush();
            }

            var reader = XmlReader.Create(new StringReader(buffer.ToString()));
            return new IncomingMessage(reader);
        }

        private OutgoingMessage Handle(IncomingMessage request)
        {
            return InvokeHandlers(request);
        }

        private OutgoingMessage InvokeHandlers(IncomingMessage incomingMessage)
        {
            return _handlers
                .Select(x => x.Handle(incomingMessage))
                .First(x => x != null);
        }
    }
}