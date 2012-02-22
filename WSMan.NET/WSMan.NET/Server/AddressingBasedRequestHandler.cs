using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public abstract class AddressingBasedRequestHandler : IRequestHandler
    {
        public OutgoingMessage Handle(IncomingMessage request)
        {
            var actionHeader = request.GetHeader<ActionHeader>();
            var messageIdHeader = request.GetHeader<MessageIdHeader>();

            var outgoingMessage = ProcessMessage(request, actionHeader);
            if (outgoingMessage == null)
            {
                return null;
            }
            outgoingMessage.AddHeader(MessageIdHeader.CreateRandom(), false);
            if (messageIdHeader != null)
            {
                outgoingMessage.AddHeader(new RelatesToHeader(messageIdHeader.MessageId), false);
            }
            outgoingMessage.AddHeader(ToHeader.Anonymous, false);
            return outgoingMessage;
        }

        protected abstract OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader);
    }
}