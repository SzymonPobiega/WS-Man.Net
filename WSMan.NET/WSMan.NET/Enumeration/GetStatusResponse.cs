using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   
   [Serializable]
   
   
   [XmlType(AnonymousType = true, Namespace = Const.Namespace)]
   [XmlRoot(Namespace = Const.Namespace, IsNullable = false)]
   public class GetStatusResponse
   {
      private XmlAttribute[] anyAttrField;
      private XmlElement[] anyField;
      private string expiresField;


      public string Expires
      {
         get { return expiresField; }
         set { expiresField = value; }
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