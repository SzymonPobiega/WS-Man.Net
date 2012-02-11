using System.Xml;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public class CreateResponseBodyWriter : IBodyWriter
    {
        private readonly EndpointReference _body;

        public CreateResponseBodyWriter(EndpointReference body)
        {
            _body = body;
        }

        public void OnWriteBodyContents(XmlWriter writer)
        {
            writer.WriteStartElement(Constants.CreateResponse_ResourceCreatedElement, Constants.Namespace);
            _body.WriteOuterXml(writer);
            writer.WriteEndElement();
        }
    }
}