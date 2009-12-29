using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [Serializable]
   [XmlType(AnonymousType = true, Namespace = Const.Namespace)]
   [XmlRoot(Namespace = Const.Namespace, IsNullable = false)]
   public class RenewResponse
   {
      private XmlAttribute[] anyAttrField;
      private XmlElement[] anyField;
      private EnumerationContext enumerationContextField;
      private string expiresField;


      public string Expires
      {
         get { return expiresField; }
         set { expiresField = value; }
      }


      public EnumerationContext EnumerationContext
      {
         get { return enumerationContextField; }
         set { enumerationContextField = value; }
      }


      [XmlAnyElement]
      public XmlElement[] Any
      {
         get { return anyField; }
         set { anyField = value; }
      }


      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr
      {
         get { return anyAttrField; }
         set { anyAttrField = value; }
      }
   }
}