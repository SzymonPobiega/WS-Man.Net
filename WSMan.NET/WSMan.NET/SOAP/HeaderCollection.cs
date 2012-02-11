using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WSMan.NET.SOAP
{
    public class HeaderCollection
    {
        private readonly List<MessageHeader> _headers = new List<MessageHeader>();

        public MessageHeader GetHeader(XName name)
        {
            return _headers.FirstOrDefault(x => x.Name == name);
        }

        public T GetHeader<T>()
            where T : class, IMessageHeader, new()
        {
            var typedHeader = new T();
            var rawHeader = _headers.FirstOrDefault(x => x.Name == typedHeader.Name);
            if (rawHeader == null)
            {
                return null;
            }
            typedHeader.Read(rawHeader.Content);
            return typedHeader;
        }

        public bool IsEmpty
        {
            get { return _headers.Count == 0; }
        }

        public void AddHeader(MessageHeader header)
        {
            _headers.Add(header);
        }

        public void AddHeader(IMessageHeader typedHeader, bool mustUnderstand)
        {
            var header = new MessageHeader(typedHeader.Name, typedHeader.Write(), mustUnderstand);
            _headers.Add(header);
        }

        public void Read(XmlReader reader, XName terminatingElementName)
        {
            while (reader.NodeType != XmlNodeType.EndElement && reader.Name() != terminatingElementName)
            {
                var headerElement = (XElement)XNode.ReadFrom(reader);
                var header = new MessageHeader(headerElement);
                _headers.Add(header);
            }
        }

        public void Write(XmlWriter output)
        {
            foreach (var header in _headers)
            {
                header.Write(output);
            }
        }
    }
}