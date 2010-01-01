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
      [XmlElement(Namespace = Const.Namespace)]      
      public EnumerationItemList Items { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlElement(Namespace = Const.Namespace)]
      public EndOfSequence EndOfSequence { get; set; }
   }
}