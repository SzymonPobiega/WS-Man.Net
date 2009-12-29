using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   [XmlType(Namespace = Const.Namespace)]
   public class EnumerationContext
   {
      [XmlText]
      public string Text { get; set; }

      public EnumerationContext()
      {         
      }

      public EnumerationContext(string value)
      {
         Text = value;
      }

      public static EnumerationContext Unique()
      {
         return new EnumerationContext(Guid.NewGuid().ToString());
      }

      public override int GetHashCode()
      {
         return Text.GetHashCode();
      }

      public override bool Equals(object obj)
      {
         EnumerationContext other = obj as EnumerationContext;
         return other != null && Text.Equals(other.Text);
      }
   }
}