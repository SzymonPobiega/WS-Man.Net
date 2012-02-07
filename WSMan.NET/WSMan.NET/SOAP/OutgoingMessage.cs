using System.Collections.Generic;
using System.Xml;

namespace WSMan.NET.SOAP
{
    public class OutgoingMessage
    {
        private IBodyWriter _bodyWriter;
        private readonly List<MessageHeader> _headers = new List<MessageHeader>();

        public OutgoingMessage AddHeader(MessageHeader header)
        {
            _headers.Add(header);
            return this;
        }

        public OutgoingMessage SetBody(IBodyWriter bodyWriter)
        {
            _bodyWriter = bodyWriter;
            return this;
        }

        public OutgoingMessage AddHeader(IMessageHeader typedHeader, bool mustUnderstand)
        {
            var header = new MessageHeader(typedHeader.Name, typedHeader.Write(), mustUnderstand);
            _headers.Add(header);
            return this;
        }

        public void Write(XmlWriter output)
        {
            output.WriteStartElement(Constants.Envelope);
            output.WriteStartElement(Constants.Header);
            foreach (var header in _headers)
            {
                header.Write(output);
            }
            output.WriteEndElement();
            output.WriteStartElement(Constants.Body);
            if (_bodyWriter != null)
            {
                _bodyWriter.OnWriteBodyContents(output);
            }
            output.WriteEndElement();
            output.WriteEndElement();
        }
    }
}