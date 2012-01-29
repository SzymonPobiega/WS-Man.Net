using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public class SerializerBodyWriter : IBodyWriter
    {
        private readonly object _toSerialize;

        public SerializerBodyWriter(object toSerialize)
        {
            _toSerialize = toSerialize;
        }

        public void OnWriteBodyContents(XmlWriter writer)
        {
            if (_toSerialize == null)
            {
                return;
            }
            var serializable = _toSerialize as IXmlSerializable;
            if (serializable != null)
            {
                serializable.WriteXml(writer);
            }
            else
            {
                var xs = new XmlSerializer(_toSerialize.GetType());
                xs.Serialize(writer, _toSerialize);
            }
        }
    }
}