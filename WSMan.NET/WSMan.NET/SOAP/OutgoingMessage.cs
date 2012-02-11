using System;
using System.Collections.Generic;
using System.Xml;

namespace WSMan.NET.SOAP
{
    public class OutgoingMessage
    {
        private IBodyWriter _bodyWriter;
        private readonly HeaderCollection _headers = new HeaderCollection();

        public OutgoingMessage AddHeader(MessageHeader header)
        {
            _headers.AddHeader(header);
            return this;
        }

        public OutgoingMessage SetBody(IBodyWriter bodyWriter)
        {
            _bodyWriter = bodyWriter;
            return this;
        }

        public OutgoingMessage AddHeader(IMessageHeader typedHeader, bool mustUnderstand)
        {
            _headers.AddHeader(typedHeader, mustUnderstand);
            return this;
        }

        public void Write(XmlWriter output)
        {
            output.WriteStartElement(Constants.Envelope);
            output.WriteStartElement(Constants.Header);
            _headers.Write(output);
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