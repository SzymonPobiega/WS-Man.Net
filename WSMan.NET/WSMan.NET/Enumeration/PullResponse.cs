using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    [XmlRoot(ElementName = "PullResponse", Namespace = Constants.NamespaceName)]
    public class PullResponse
    {
        [XmlElement(Namespace = Constants.NamespaceName)]
        public EnumerationContextKey EnumerationContext { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public EnumerationItemList Items { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public EndOfSequence EndOfSequence { get; set; }
    }
}