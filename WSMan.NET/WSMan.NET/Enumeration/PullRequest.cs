using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{   
   [MessageContract(IsWrapped = true, WrapperName = "Pull", WrapperNamespace = Const.Namespace)]   
   public class PullRequest
   {
      [MessageBodyMember(Order = 0)]
      [XmlElement(Namespace = Const.Namespace)]
      public EnumerationContextKey EnumerationContext { get; set; }

      [MessageBodyMember(Order = 1)]
      [XmlElement(Namespace = Const.Namespace)]
      public MaxTime MaxTime { get; set; }

      [MessageBodyMember(Order = 2)]
      [XmlElement(Namespace = Management.Const.Namespace)]
      public MaxElements MaxElements { get; set; }      
   }
}