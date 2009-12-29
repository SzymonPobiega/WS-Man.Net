using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Eventing
{
   [MessageContract(IsWrapped = true, WrapperName = "Unsubscribe", WrapperNamespace = Const.Namespace)]   
   public class UnsubscribeRequest
   {
      [XmlAnyElement]
      public XmlElement[] Any { get; set; }

      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }
   }
}