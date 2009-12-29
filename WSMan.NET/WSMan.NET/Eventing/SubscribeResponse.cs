using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   [MessageContract(IsWrapped = true, WrapperName = "SubscribeResponse", WrapperNamespace = Const.Namespace)]
   public class SubscribeResponse
   {
      [MessageBodyMember(Order = 0)]
      [XmlElement(Namespace = Const.Namespace)]
      public EndpointReference SubscriptionManager { get; set; }      

      [MessageBodyMember(Order = 1)]
      [XmlElement(Namespace = Const.Namespace)]
      public Expires Expires { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlElement(Namespace = Enumeration.Const.Namespace)]
      public EnumerationContext EnumerationContext { get; set; }

      [XmlAnyElement]
      public XmlElement[] Any { get; set; }

      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }
   }
}