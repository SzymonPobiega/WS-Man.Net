using System;
using System.Xml.Serialization;

namespace WSMan.NET.SOAP
{
    public static class OutgoingMessageExtensions
    {
        public static T GetPayload<T>(this IncomingMessage messageWithPayload)
        {
            return (T) messageWithPayload.GetPayload(typeof (T));
        }

        public static object GetPayload(this IncomingMessage messageWithPayload, Type expectedType)
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
    }
}