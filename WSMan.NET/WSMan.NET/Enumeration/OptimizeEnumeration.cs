using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [XmlType(Namespace = Const.Namespace)]
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