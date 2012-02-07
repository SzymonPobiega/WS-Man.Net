using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    [XmlRoot(ElementName = "Pull", Namespace = Constants.NamespaceName)]
    public class PullRequest
    {
        [XmlElement(Namespace = Constants.NamespaceName)]
        public EnumerationContextKey EnumerationContext { get; set; }

        [XmlElement(Namespace = Constants.NamespaceName)]
        public MaxTime MaxTime { get; set; }

        [XmlElement(Namespace = Management.Const.NamespaceName)]
        public MaxElements MaxElements { get; set; }
    }
}