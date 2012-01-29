using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Addressing
{
    public class ToHeader : IMessageHeader
    {
        private string _uri;

        public ToHeader()
        {
        }

        public ToHeader(string uri)
        {
            _uri = uri;
        }

        public XName Name
        {
            get { return Constants.Namespace + "To"; }
        }

        public string Uri
        {
            get { return _uri; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(Uri);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _uri = text.Value;
        }
    }
}