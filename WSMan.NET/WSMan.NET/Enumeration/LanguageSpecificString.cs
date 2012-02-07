using System;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    [Serializable]
    [XmlType(Namespace = Constants.NamespaceName)]
    public class LanguageSpecificString
    {
        [XmlAttribute(AttributeName = "xml:lang")]
        public string LanguageCode { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}