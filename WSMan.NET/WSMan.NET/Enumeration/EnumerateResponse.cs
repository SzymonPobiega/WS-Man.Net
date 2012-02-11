using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [XmlRoot(ElementName = "EnumerateResponse", Namespace = Constants.NamespaceName)]
   public class EnumerateResponse
   {
      [XmlElement(Namespace = Constants.NamespaceName)]
      public Expires Expires { get; set; }

      [XmlElement(Namespace = Constants.NamespaceName)]
      public EnumerationContextKey EnumerationContext { get; set; }

      [XmlElement(Namespace = Management.Constants.NamespaceName)]      
      public EnumerationItemList Items { get; set; }

      [XmlElement(Namespace = Management.Constants.NamespaceName)]
      public EndOfSequence EndOfSequence { get; set; }
   }
}