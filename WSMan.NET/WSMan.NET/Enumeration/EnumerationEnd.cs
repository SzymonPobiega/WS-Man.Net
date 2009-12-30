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
   
   
   [XmlType(AnonymousType = true, Namespace = Const.Namespace)]
   [XmlRoot(Namespace = Const.Namespace, IsNullable = false)]
   public class EnumerationEnd
   {
      private XmlAttribute[] anyAttrField;
      private XmlElement[] anyField;
      private string codeField;
      private EnumerationContextKey enumerationContextField;

      private LanguageSpecificString[] reasonField;


      public EnumerationContextKey EnumerationContext
      {
         get { return enumerationContextField; }
         set { enumerationContextField = value; }
      }


      public string Code
      {
         get { return codeField; }
         set { codeField = value; }
      }


      [XmlElement("Reason")]
      public LanguageSpecificString[] Reason
      {
         get { return reasonField; }
         set { reasonField = value; }
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