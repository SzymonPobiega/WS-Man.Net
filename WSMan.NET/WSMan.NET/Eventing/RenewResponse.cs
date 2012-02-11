using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
    [XmlRoot(ElementName = "RenewResponse", Namespace = Constants.NamespaceName)]
    public class RenewResponse
    {
        [XmlElement]
        public Expires Expires { get; set; }

        [XmlAnyElement]
        public XmlElement[] Any { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}