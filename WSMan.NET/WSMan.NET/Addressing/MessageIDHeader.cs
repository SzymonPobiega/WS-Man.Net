using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Addressing
{
    public class MessageIdHeader : IMessageHeader
    {
        private string _messageId;

        public MessageIdHeader()
        {
        }

        public MessageIdHeader(string messageId)
        {
            _messageId = messageId;
        }

        public static MessageIdHeader CreateRandom()
        {
            return new MessageIdHeader("uuid:"+Guid.NewGuid());
        }

        public XName Name
        {
            get { return Constants.Namespace + "MessageID"; }
        }

        public string MessageId
        {
            get { return _messageId; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(MessageId);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _messageId = text.Value;
        }
    }
}