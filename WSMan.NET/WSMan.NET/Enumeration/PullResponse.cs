using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [MessageContract(IsWrapped = true, WrapperName = "PullResponse", WrapperNamespace = Const.Namespace)]
   public class PullResponse
   {
      [MessageBodyMember(Order = 0)]
      [XmlElement(Namespace = Const.Namespace)]
      public EnumerationContextKey EnumerationContext { get; set; }

      [MessageBodyMember(Order = 1)]
      [XmlArray(ElementName = "Items", Namespace = Const.Namespace)]
      [XmlArrayItem(ElementName = "EndpointReference", Type = typeof (EndpointAddress10),
         Namespace = Const.WSAddressing200408Namespace)]
      public List<EndpointAddress10> EnumerateEPRItems { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlElement(Namespace = Const.Namespace)]
      public EndOfSequence EndOfSequence { get; set; }
   }
}