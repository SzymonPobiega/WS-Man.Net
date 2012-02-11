using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Eventing
{
    [XmlRoot(ElementName = "Unsubscribe", Namespace = Constants.NamespaceName)]
    public class UnsubscribeRequest
    {
        [XmlAnyElement]
        public XmlElement[] Any { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}