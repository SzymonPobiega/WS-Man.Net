using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   [MessageContract(IsWrapped = true, WrapperName = "Subscribe", WrapperNamespace = Const.Namespace)]   
   public class SubscribeRequest
   {
      [MessageBodyMember(Order = 0)]
      [XmlElement(Namespace = Const.Namespace)]
      public EndpointReference EndTo { get; set; }

      [MessageBodyMember(Order = 1)]
      [XmlElement(Namespace = Const.Namespace)]
      public Delivery Delivery { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlElement(Namespace = Const.Namespace)]
      public Expires Expires { get; set; }

      [MessageBodyMember(Order = 3)]
      [XmlElement(Namespace = Enumeration.Const.Namespace)]
      public Filter Filter { get; set; }

      [XmlAnyElement]
      public XmlElement[] Any { get; set; }

      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }
   }
}