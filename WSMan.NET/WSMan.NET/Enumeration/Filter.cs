using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    public class Filter : IXmlSerializable
    {
        private const string DialectAttribute = "Dialect";
        private string _dialect;
        private string _rawValue;
        private readonly object _value;

        public Filter(string dialect, object value)
        {
            _dialect = dialect;
            _value = value;
        }

        public Filter()
        {
        }
        
        public string Dialect
        {
            get { return _dialect; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            _dialect = reader.GetAttribute(DialectAttribute);
            _rawValue = reader.ReadInnerXml();
        }

        public object DeserializeAs(Type type)
        {
            if (type == typeof(void) || string.IsNullOrWhiteSpace(_rawValue))
            {
                return null;
            }
            var reader = XmlReader.Create(new StringReader(_rawValue));
            if (typeof(IXmlSerializable).IsAssignableFrom(type))
            {
                var serializable = (IXmlSerializable)Activator.CreateInstance(type);
                serializable.ReadXml(reader);
                return serializable;
            }
            var serializer = new XmlSerializer(type);
            if (!reader.IsEmptyElement)
            {
                return serializer.Deserialize(reader);
            }
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(DialectAttribute, _dialect);
            var serializable = _value as IXmlSerializable;
            if (serializable != null)
            {
                serializable.WriteXml(writer);
                return;
            }
            if (_value != null)
            {
                var serializer = new XmlSerializer(_value.GetType());
                serializer.Serialize(writer, _value);
            }
        }
    }
}