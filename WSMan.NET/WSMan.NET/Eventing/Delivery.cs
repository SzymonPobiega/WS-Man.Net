using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Eventing
{
   [XmlType(Namespace = Const.Namespace)]
   public class Delivery
   {      
      public static Delivery Pull()
      {
         return new Delivery {Mode = Const.DeliveryModePull};
      }

      [XmlAttribute(DataType = "anyURI")]
      public string Mode { get; set; }

      [XmlAnyElement]
      public XmlNode[] Any { get; set; }

      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }
   }
}