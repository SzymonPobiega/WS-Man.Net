using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using WSMan.NET.SOAP;

namespace WSMan.NET.Addressing
{
    public class EndpointReference : IXmlSerializable
    {
        private static readonly XName EndpointRefereceElement = Constants.Namespace + "EndpointReference";
        private static readonly XName AddressElement = Constants.Namespace + "Address";
        private static readonly XName ReferencePropertiesElement = Constants.Namespace + "ReferenceProperties";

        private string _address;
        private readonly HeaderCollection _properties = new HeaderCollection();
       
        public EndpointReference()
        {
        }

        public EndpointReference(string address)
        {
            _address = address;
        }

        public EndpointReference AddProperty(MessageHeader messageHeader)
        {
            _properties.AddHeader(messageHeader);
            return this;
        }

        public EndpointReference AddProperty(IMessageHeader typedHeader, bool mustUnderstand)
        {
            _properties.AddHeader(typedHeader, mustUnderstand);
            return this;
        }

        public MessageHeader GetProperty(XName name)
        {
            return _properties.GetHeader(name);
        }

        public T GetProperty<T>() where T : class, IMessageHeader, new()
        {
            return _properties.GetHeader<T>();
        }


        public string Address
        {
            get { return _address; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadOuterXml(XmlReader reader)
        {
            reader.ReadStartElement(EndpointRefereceElement);
            ReadAddress(reader);
            ReadProperties(reader);
            reader.ReadEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            ReadAddress(reader);
            ReadProperties(reader);
            reader.ReadEndElement();
        }

        private void ReadProperties(XmlReader reader)
        {
            if (reader.Name() == ReferencePropertiesElement)
            {
                reader.ReadStartElement(ReferencePropertiesElement);
                _properties.Read(reader, ReferencePropertiesElement);
                reader.ReadEndElement();
            }
        }

        private void ReadAddress(XmlReader reader)
        {
            reader.ReadStartElement(AddressElement);
            _address = reader.Value;
            reader.Read();
            reader.ReadEndElement();
        }

        public void WriteOuterXml(XmlWriter writer)
        {
            writer.WriteStartElement(EndpointRefereceElement);
            WriteXml(writer);
            writer.WriteEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            WriteAddress(writer);
            WriteProperties(writer);
        }

        private void WriteProperties(XmlWriter writer)
        {
            if (!_properties.IsEmpty)
            {
                writer.WriteStartElement(ReferencePropertiesElement);
                _properties.Write(writer);
                writer.WriteEndElement();
            }
        }

        private void WriteAddress(XmlWriter writer)
        {
            writer.WriteStartElement(AddressElement);
            writer.WriteString(_address);
            writer.WriteEndElement();
        }

        public override string ToString()
        {
            return _address;
        }

    }
}