using WSMan.NET.Addressing;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public class TransferServer : AddressingBasedRequestHandler
    {
        private readonly ITransferRequestHandler _handler;
        private readonly MessageFactory _factory;

        public TransferServer(ITransferRequestHandler handler)
        {
            _handler = handler;
            _factory = new MessageFactory();
        }
        
        protected override OutgoingMessage ProcessMessage(IncomingMessage request, ActionHeader actionHeader)
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
                    return null;
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
            var payload = _handler.HandlePut(incomingHeaders, outgoingHeaders, x => OutgoingMessageExtensions.GetPayload(putRequest, x));
            response.SetBody(new SerializerBodyWriter(payload));
            return response;
        }

        private OutgoingMessage Create(IncomingMessage createRequest)
        {
            var response = _factory.CreateCreateResponse();
            var incomingHeaders = new IncomingHeaders(createRequest);
            var outgoingHeaders = new OutgoingHeaders(response);
            var reference = _handler.HandleCreate(incomingHeaders, outgoingHeaders, x => OutgoingMessageExtensions.GetPayload(createRequest, x));
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

        
    }
}