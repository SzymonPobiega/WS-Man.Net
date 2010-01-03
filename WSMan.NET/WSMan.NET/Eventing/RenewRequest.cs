using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   [MessageContract(IsWrapped = true, WrapperName = "Renew", WrapperNamespace = Const.Namespace)]
   public class RenewRequest
   {
      [MessageBodyMember(Order = 0)]
      [XmlElement]
      public Expires Expires { get; set; }

      [MessageBodyMember(Order = 1)]
      [XmlAnyElement]
      public XmlElement[] Any { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }
   }
}