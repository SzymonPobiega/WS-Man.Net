using System;
using System.Xml.Serialization;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public class MessageFactory
    {
        public OutgoingMessage CreateGetRequest()
        {
            return CreateMessage(Constants.GetAction);
        }

        public OutgoingMessage CreateGetResponse()
        {
            return CreateMessage(Constants.GetResponseAction);
        }

        public OutgoingMessage CreatePutRequest()
        {
            return CreateMessage(Constants.PutAction);
        }

        public OutgoingMessage CreatePutResponse()
        {
            return CreateMessage(Constants.PutResponseAction);
        }

        public OutgoingMessage CreateCreateRequest()
        {
            return CreateMessage(Constants.CreateAction);
        }

        public OutgoingMessage CreateCreateResponse()
        {
            return CreateMessage(Constants.CreateResponseAction);
        }

        public OutgoingMessage CreateDeleteRequest()
        {
            return CreateMessage(Constants.DeleteAction);
        }

        public OutgoingMessage CreateDeleteResponse()
        {
            return CreateMessage(Constants.DeleteResponseAction);
        }

        public EndpointReference DeserializeCreateResponse(IncomingMessage createResponseMessage)
        {
            var reader = createResponseMessage.GetReaderAtBodyContents();
            reader.ReadStartElement(Constants.CreateResponse_ResourceCreatedElement, Constants.Namespace);
            var result = new EndpointReference();
            result.ReadXml(reader);
            reader.ReadEndElement();
            return result;
        }

        public object DeserializeMessageWithPayload(IncomingMessage messageWithPayload, Type expectedType)
        {
            if (messageWithPayload.IsEmpty)
            {
                return null;
            }
            if (typeof(IXmlSerializable).IsAssignableFrom(expectedType))
            {
                var serializable = (IXmlSerializable)Activator.CreateInstance(expectedType);
                serializable.ReadXml(messageWithPayload.GetReaderAtBodyContents());
                return serializable;
            }
            var xs = new XmlSerializer(expectedType);
            return xs.Deserialize(messageWithPayload.GetReaderAtBodyContents());
        }

        public OutgoingMessage CreateMessage(string action)
        {
            var respose = new OutgoingMessage();
            respose.AddHeader(new ActionHeader(action), true);
            return respose;
        }
    }
}