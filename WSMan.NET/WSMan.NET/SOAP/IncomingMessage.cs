using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WSMan.NET.SOAP
{
    public class IncomingMessage : IDisposable
    {
        private bool _empty;
        private readonly XmlReader _reader;
        private XmlReader _bodyReader;
        private readonly HeaderCollection _headers = new HeaderCollection();

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
            _headers.Read(_reader, Constants.Body); 
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
            if (_bodyReader == null)
            {
                _bodyReader = XmlReader.Create(_reader, new XmlReaderSettings());
            }
            return _bodyReader;
        }

        public MessageHeader GetHeader(XName name)
        {
            return _headers.GetHeader(name);
        }

        public T GetHeader<T>()
            where T : class, IMessageHeader, new()
        {
            return _headers.GetHeader<T>();
        }

        public void Dispose()
        {
        }
    }
}