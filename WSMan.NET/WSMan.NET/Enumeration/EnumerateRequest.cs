using System.Xml.Serialization;
using WSMan.NET.Addressing;

namespace WSMan.NET.Enumeration
{
    [XmlRoot(ElementName = "Enumerate", Namespace = Constants.NamespaceName)]
    public class EnumerateRequest
    {
        [XmlElement(Namespace = Constants.NamespaceName)]
        public EndpointReference EndTo { get; set; }

        [XmlElement(Namespace = Management.Constants.NamespaceName)]
        public Filter Filter { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public Expires Expires { get; set; }

        [XmlElement(Namespace = Management.Constants.NamespaceName)]
        public EnumerationMode EnumerationMode { get; set; }

        [XmlElement(Namespace = Management.Constants.NamespaceName)]
        public OptimizeEnumeration OptimizeEnumeration { get; set; }

        [XmlElement(Namespace = Management.Constants.NamespaceName)]
        public MaxElements MaxElements { get; set; }

    }
}