using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   public class ItemList<T> : IXmlSerializable
   {
      private List<T> _elements;

      public ItemList()
      {         
      }

      public ItemList(List<T> elements)
      {
         _elements = elements;
      }

      public List<T> Elements
      {
         get { return _elements; }
      }

      public XmlSchema GetSchema()
      {
         return null;
      }

      public void ReadXml(XmlReader reader)
      {
         //XmlSerializer 
      }

      public void WriteXml(XmlWriter writer)
      {
         throw new NotImplementedException();
      }
   }
}