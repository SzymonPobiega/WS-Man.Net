using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [MessageContract(IsWrapped = false)]
   [Serializable]
   [XmlType(AnonymousType = true, Namespace = Const.Namespace)]
   [XmlRoot(Namespace = Const.Namespace, IsNullable = false)]
   public class PullResponse
   {
      private EndOfSequence endOfSequenceField;
      private EnumerationContext enumerationContextField;      

      [MessageBodyMember]
      public EnumerationContext EnumerationContext
      {
         get { return enumerationContextField; }
         set { enumerationContextField = value; }
      }

      [MessageBodyMember]
      [XmlArray(ElementName = "Items")]
      [XmlArrayItem(ElementName = "EndpointReference", Type = typeof(EndpointAddress10), Namespace = Const.WSAddressing200408Namespace)]
      public List<EndpointAddress10> EnumerateEPRItems { get; set; }

      [MessageBodyMember]
      public EndOfSequence EndOfSequence
      {
         get { return endOfSequenceField; }
         set { endOfSequenceField = value; }
      }
   }
}