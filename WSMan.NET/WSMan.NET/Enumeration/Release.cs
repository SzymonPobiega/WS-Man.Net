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
   public class Release
   {
      private XmlAttribute[] anyAttrField;
      private EnumerationContextKey enumerationContextField;


      public EnumerationContextKey EnumerationContext
      {
         get { return enumerationContextField; }
         set { enumerationContextField = value; }
      }


      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr
      {
         get { return anyAttrField; }
         set { anyAttrField = value; }
      }
   }
}