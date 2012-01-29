using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using WSMan.NET.Transfer;

namespace WSMan.NET.SOAP
{
    public class IncomingMessage : IDisposable
    {
        private bool _empty;
        private readonly XmlReader _reader;
        private readonly List<MessageHeader> _headers = new List<MessageHeader>();

        public IncomingMessage(XmlReader reader)
        {
            _reader = XmlReader.Create(reader, new XmlReaderSettings
                                                   {
                                                       IgnoreWhitespace = true
                                                   });
            ReadHeaders();
        }

        private void ReadHeaders()
        {
            _reader.ReadStartElement(Constants.Envelope);
            _reader.ReadStartElement(Constants.Header);
            while (_reader.NodeType != XmlNodeType.EndElement && _reader.Name() != Constants.Body)
            {
                var headerElement = (XElement)XNode.ReadFrom(_reader);
                var header = new MessageHeader(headerElement);
                _headers.Add(header);
            } 
            if (_reader.NodeType == XmlNodeType.EndElement)
            {
                _reader.ReadEndElement();
            }
            _reader.ReadStartElement(Constants.Body);
            _empty = _reader.NodeType == XmlNodeType.EndElement;
        }

        public bool IsEmpty
        {
            get { return _empty; }
        }

        public XmlReader GetReaderAtBodyContents()
        {
            if (_empty)
            {
                throw new InvalidOperationException("Can't get body reader for message with empty body.");
            }
            return XmlReader.Create(_reader, new XmlReaderSettings());
        }

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

        public void Dispose()
        {
        }
    }
}