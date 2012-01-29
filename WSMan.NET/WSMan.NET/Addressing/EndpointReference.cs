using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Addressing
{
    public class EndpointReference : IXmlSerializable
    {
        private string _address;

        private static readonly XName EndpointRefereceElement = Constants.Namespace + "EndpointReference";
        private static readonly XName AddressElement = Constants.Namespace + "Address";

        public EndpointReference()
        {
        }

        public EndpointReference(string address)
        {
            _address = address;
        }

        public string Address
        {
            get { return _address; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(EndpointRefereceElement);
            ReadAddress(reader);
            reader.ReadEndElement();
        }

        private void ReadAddress(XmlReader reader)
        {
            reader.ReadStartElement(AddressElement);
            _address = reader.Value;
            reader.Read();
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(EndpointRefereceElement);
            WriteAddress(writer);
            writer.WriteEndElement();
        }

        private void WriteAddress(XmlWriter writer)
        {
            writer.WriteStartElement(AddressElement);
            writer.WriteString(_address);
            writer.WriteEndElement();
        }
    }
}