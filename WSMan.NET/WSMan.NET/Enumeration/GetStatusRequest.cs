using System;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Constants.NamespaceName)]
    [XmlRoot("GetStatus", Namespace = Constants.NamespaceName, IsNullable = false)]
    public class GetStatusRequest
    {
        [XmlElement]
        public EnumerationContextKey EnumerationContext { get; set; }

        [XmlAnyElement]
        public XmlElement[] Any { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}