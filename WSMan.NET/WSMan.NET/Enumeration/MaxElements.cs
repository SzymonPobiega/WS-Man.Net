using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{   
   [XmlType(Namespace = Management.Const.NamespaceName)]
   public class MaxElements
   {
      [XmlText]
      public int Value { get; set; }
      
      public MaxElements()
      {         
      }

      public MaxElements(int value)
      {
         Value = value;
      }

      public XmlSchema GetSchema()
      {
         return null;
      }

      public void ReadXml(XmlReader reader)
      {
         Value = reader.ReadElementContentAsInt();
      }

      public void WriteXml(XmlWriter writer)
      {
         writer.WriteValue(Value);
      }
   }
}