using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [XmlType(Namespace = Constants.NamespaceName)]
   public class OptimizeEnumeration
   {      
      public static OptimizeEnumeration True
      {
         get { return new OptimizeEnumeration(); }
      }
      public static OptimizeEnumeration False
      {
         get { return null;}
      }
   }
}