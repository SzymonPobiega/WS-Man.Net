using System;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public class TransferServer
    {
        private readonly ITransferRequestHandler _handler;
        private readonly MessageFactory _factory;

        public TransferServer(ITransferRequestHandler handler)
        {
            _handler = handler;
            _factory = new MessageFactory();
        }

        public OutgoingMessage Handle(IncomingMessage request)
        {
            var actionHeader = request.GetHeader<ActionHeader>();
            var messageIdHeader = request.GetHeader<MessageIdHeader>();

            var outgoingMessage = ProcessMessage(request, actionHeader);

            outgoingMessage.AddHeader(MessageIdHeader.CreateRandom(), false);
            outgoingMessage.AddHeader(new RelatesToHeader(messageIdHeader.MessageId),false);
            outgoingMessage.AddHeader(ToHeader.Anonymous, false);
            return outgoingMessage;
        }

        private OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
        {
            switch (actionHeader.Action)
            {
                case Constants.CreateAction:
                    return Create(request);
                case Constants.GetAction:
                    return Get(request);
                case Constants.PutAction:
                    return Put(request);
                case Constants.DeleteAction:
                    return Delete(request);
                default:
                    throw new NotSupportedException();
            }
        }

        private OutgoingMessage Get(IncomingMessage getRequest)
        {
            var response =  _factory.CreateGetResponse();          
            var incomingHeaders = new IncomingHeaders(getRequest);
            var outgoingHeaders = new OutgoingHeaders(response);
            var payload = _handler.HandleGet(incomingHeaders, outgoingHeaders);
            response.SetBody(new SerializerBodyWriter(payload));
            return response;
        }

        private OutgoingMessage Put(IncomingMessage putRequest)
        {
            var response = _factory.CreatePutResponse();
            var incomingHeaders = new IncomingHeaders(putRequest);
            var outgoingHeaders = new OutgoingHeaders(response);
            var payload = _handler.HandlePut(incomingHeaders, outgoingHeaders, x => _factory.DeserializeMessageWithPayload(putRequest, x));
            response.SetBody(new SerializerBodyWriter(payload));
            return response;
        }

        private OutgoingMessage Create(IncomingMessage createRequest)
        {
            var response = _factory.CreateCreateResponse();
            var incomingHeaders = new IncomingHeaders(createRequest);
            var outgoingHeaders = new OutgoingHeaders(response);
            var reference = _handler.HandleCreate(incomingHeaders, outgoingHeaders, x => _factory.DeserializeMessageWithPayload(createRequest, x));
            response.SetBody(new CreateResponseBodyWriter(reference));
            return response;
        }

        private OutgoingMessage Delete(IncomingMessage deleteRequest)
        {
            var response = _factory.CreateDeleteResponse();
            var incomingHeaders = new IncomingHeaders(deleteRequest);
            var outgoingHeaders = new OutgoingHeaders(response);
            _handler.HandlerDelete(incomingHeaders, outgoingHeaders);
            return response;
        }

        private class OutgoingHeaders : IOutgoingHeaders
        {
            private readonly OutgoingMessage _outgoingMessage;

            public OutgoingHeaders(OutgoingMessage outgoingMessage)
            {
                _outgoingMessage = outgoingMessage;
            }

            public void AddHeader(IMessageHeader header, bool mustUnderstand)
            {
                _outgoingMessage.AddHeader(header, mustUnderstand);
            }
        }

        private class IncomingHeaders : IIncomingHeaders
        {
            private readonly IncomingMessage _incomingMessage;

            public IncomingHeaders(IncomingMessage incomingMessage)
            {
                _incomingMessage = incomingMessage;
            }

            public T GetHeader<T>() where T : class, IMessageHeader, new()
            {
                return _incomingMessage.GetHeader<T>();
            }
        }
    }
}