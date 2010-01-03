using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [MessageContract(IsWrapped = true, WrapperName = "Enumerate", WrapperNamespace = Const.Namespace)]   
   public class EnumerateRequest
   {
      [MessageBodyMember(Order = 0)]      
      [XmlElement(Namespace = Const.Namespace)]
      public EndpointAddress10 EndTo { get; set; }

      [MessageBodyMember(Order = 1)]      
      [XmlElement(Namespace = Management.Const.Namespace)]
      public Filter Filter { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlElement(Namespace = Const.Namespace)]
      public Expires Expires { get; set; }      

      [MessageBodyMember(Order = 3)]      
      [XmlElement(Namespace = Management.Const.Namespace)]
      public EnumerationMode EnumerationMode { get; set; }

      [MessageBodyMember(Order = 4)]
      [XmlElement(Namespace = Management.Const.Namespace)]
      public OptimizeEnumeration OptimizeEnumeration { get; set; }

      [MessageBodyMember(Order = 5)]
      [XmlElement(Namespace = Management.Const.Namespace)]
      public MaxElements MaxElements { get; set; }      

   }
}