using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Management
{
    public class ResourceUriHeader : IMessageHeader
    {
        private string _resourceUri;

        public ResourceUriHeader()
        {
        }

        public ResourceUriHeader(string resourceUri)
        {
            _resourceUri = resourceUri;
        }

        public XName Name
        {
            get { return Const.Namespace + "ResourceURI"; }
        }

        public string ResourceUri
        {
            get { return _resourceUri; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(ResourceUri);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _resourceUri = text.Value;
        }

    }
}