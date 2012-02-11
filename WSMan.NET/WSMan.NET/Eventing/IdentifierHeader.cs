using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Eventing
{
    public class IdentifierHeader : IMessageHeader
    {
        private string _identifier;

        public IdentifierHeader()
        {
        }

        public IdentifierHeader(string identifier)
        {
            _identifier = identifier;
        }

        public XName Name
        {
            get { return Constants.Namespace + "Identifier"; }
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(Identifier);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _identifier = text.Value;
        }

    }
}