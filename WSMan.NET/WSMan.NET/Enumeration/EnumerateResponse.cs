using System;

using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{   
   [Serializable]
   [MessageContract(IsWrapped = true, WrapperName = "EnumerateResponse", WrapperNamespace = Const.Namespace)]
   public class EnumerateResponse
   {
      [MessageBodyMember]
      [XmlElement(Namespace = Const.Namespace)]
      public Expires Expires { get; set; }

      [MessageBodyMember]
      [XmlElement(Namespace = Const.Namespace)]
      public EnumerationContext EnumerationContext { get; set; }

      [MessageBodyMember]
      [XmlArray(ElementName = "Items", Namespace = Management.Const.Namespace)]
      [XmlArrayItem(ElementName = "EndpointReference", Type = typeof(EndpointAddress10), Namespace = Const.WSAddressing200408Namespace)]
      public List<EndpointAddress10> EnumerateEPRItems { get; set; }

      [MessageBodyMember, XmlElement(Namespace = Management.Const.Namespace)]
      public EndOfSequence EndOfSequence { get; set; }
   }
}