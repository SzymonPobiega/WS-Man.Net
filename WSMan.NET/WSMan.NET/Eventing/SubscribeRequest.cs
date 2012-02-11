using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
    [XmlRoot(ElementName = "Subscribe", Namespace = Constants.NamespaceName)]
    public class SubscribeRequest
    {
        [XmlElement(Namespace = Constants.NamespaceName)]
        public EndpointReference EndTo { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public Delivery Delivery { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public Expires Expires { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public Filter Filter { get; set; }

        [XmlAnyElement]
        public XmlElement[] Any { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}