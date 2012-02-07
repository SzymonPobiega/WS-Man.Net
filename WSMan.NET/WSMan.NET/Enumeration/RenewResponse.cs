using System;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [Serializable]
   [XmlType(AnonymousType = true, Namespace = Constants.NamespaceName)]
   [XmlRoot(Namespace = Constants.NamespaceName, IsNullable = false)]
   public class RenewResponse
   {
       [XmlElement]
       public string Expires { get; set; }

       [XmlElement]
       public EnumerationContextKey EnumerationContext { get; set; }

       [XmlAnyElement]
       public XmlElement[] Any { get; set; }


       [XmlAnyAttribute]
       public XmlAttribute[] AnyAttr { get; set; }
   }
}