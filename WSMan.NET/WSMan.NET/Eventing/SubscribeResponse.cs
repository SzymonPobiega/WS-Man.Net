using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Addressing;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   [XmlRoot(ElementName = "SubscribeResponse", Namespace = Constants.NamespaceName)]
   public class SubscribeResponse
   {      
      [XmlElement(Namespace = Constants.NamespaceName)]
      public EndpointReference SubscriptionManager { get; set; }      
    
      [XmlElement(Namespace = Constants.NamespaceName)]
      public Expires Expires { get; set; }

      [XmlElement(Namespace = Enumeration.Constants.NamespaceName)]
      public EnumerationContextKey EnumerationContext { get; set; }

      [XmlAnyElement]
      public XmlElement[] Any { get; set; }

      [XmlAnyAttribute]
      public XmlAttribute[] AnyAttr { get; set; }
   }
}