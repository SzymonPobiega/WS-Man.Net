using WSMan.NET.Addressing;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public static class TransferClient
    {
        public static T Get<T>(this IMessageBuilder messageBuilder)
        {
            var responseMessage = messageBuilder
                .AddHeader(new ActionHeader(Constants.GetAction), true)
                .SendAndGetResponse();

            return responseMessage.GetPayload<T>();
        }

        public static T Put<T>(this IMessageBuilder messageBuilder, object payload)
        {
            var responseMessage = messageBuilder
                .AddHeader(new ActionHeader(Constants.PutAction), true)
                .AddBody(payload)
                .SendAndGetResponse();

            return responseMessage.GetPayload<T>();
        }

        public static EndpointReference Create(this IMessageBuilder messageBuilder, object payload)
        {
            var responseMessage = messageBuilder
                .AddHeader(new ActionHeader(Constants.CreateAction), true)
                .AddBody(payload)
                .SendAndGetResponse();

            return DeserializeCreateResponse(responseMessage);
        }

        private static EndpointReference DeserializeCreateResponse(IncomingMessage createResponseMessage)
        {
            var reader = createResponseMessage.GetReaderAtBodyContents();
            reader.ReadStartElement(Constants.CreateResponse_ResourceCreatedElement, Constants.Namespace);
            var result = new EndpointReference();
            result.ReadOuterXml(reader);
            return result;
        }

        public static void Delete(this IMessageBuilder messageBuilder)
        {
            messageBuilder
                .AddHeader(new ActionHeader(Constants.DeleteAction), true)
                .SendAndGetResponse();
        }
    }
}