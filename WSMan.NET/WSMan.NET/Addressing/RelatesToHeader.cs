using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Addressing
{
    public class RelatesToHeader : IMessageHeader
    {
        private string _relatedMessageId;

        public RelatesToHeader()
        {
        }

        public RelatesToHeader(string relatedMessageId)
        {
            _relatedMessageId = relatedMessageId;
        }

        public XName Name
        {
            get { return Constants.Namespace + "RelatesTo"; }
        }

        public string RelatedMessageId
        {
            get { return _relatedMessageId; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(RelatedMessageId);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _relatedMessageId = text.Value;
        }
    }
}