using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.WCF
{
    internal class SerializerBodyWriter : BodyWriter
    {
        private readonly object _toSerialize;

        public SerializerBodyWriter(object toSerialize)
            : base(false)
        {
            _toSerialize = toSerialize;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            if (_toSerialize != null)
            {
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
}