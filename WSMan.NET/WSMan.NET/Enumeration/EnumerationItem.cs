
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using WSMan.NET.Addressing;

namespace WSMan.NET.Enumeration
{
    public class EnumerationItem : IXmlSerializable
    {
        private readonly object _objectValue;
        private string _rawValue;
        private EndpointReference _eprValue;
        private EnumerationMode _mode;
        private static readonly XName ItemElementName = Management.Const.Namespace + "Item";

        public EndpointReference EPRValue
        {
            get { return _eprValue; }
        }

        public EnumerationItem()
        {
            _mode = EnumerationMode.EnumerateEPR;
        }

        public EnumerationItem(EndpointReference epr, object value)
        {
            _eprValue = epr;
            _objectValue = value;
            _mode = EnumerationMode.EnumerateObjectAndEPR;
        }

        public EnumerationItem(EndpointReference epr)
        {
            _eprValue = epr;
            _mode = EnumerationMode.EnumerateEPR;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public object DeserializeAs(Type type)
        {
            if (type == typeof (void))
            {
                return null;
            }
            var reader = XmlReader.Create(new StringReader(_rawValue));
            var serializer = new XmlSerializer(type);
            return serializer.Deserialize(reader);
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.Name() == ItemElementName)
            {
                _mode = EnumerationMode.EnumerateObjectAndEPR;
                reader.ReadStartElement(ItemElementName);
                _rawValue = reader.ReadOuterXml();
            }
            _eprValue = new EndpointReference();
            _eprValue.ReadXml(reader);
            if (IncludeObject())
            {
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (IncludeObject())
            {
                writer.WriteStartElement(ItemElementName);
                var serializer = new XmlSerializer(_objectValue.GetType());
                serializer.Serialize(writer, _objectValue);
            }
            _eprValue.WriteXml(writer);
            if (IncludeObject())
            {
                writer.WriteEndElement();
            }
        }

        private bool IncludeObject()
        {
            return _mode == EnumerationMode.EnumerateObjectAndEPR;
        }
    }
}