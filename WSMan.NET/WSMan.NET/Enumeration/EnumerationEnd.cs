using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = Constants.NamespaceName)]
    [XmlRoot(Namespace = Constants.NamespaceName, IsNullable = false)]
    public class EnumerationEnd
    {
        [XmlElement]
        public EnumerationContextKey EnumerationContext { get; set; }

        [XmlElement]
        public string Code { get; set; }

        [XmlElement("Reason")]
        public LanguageSpecificString[] Reason { get; set; }


        [XmlAnyElement]
        public XmlElement[] Any { get; set; }


        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}