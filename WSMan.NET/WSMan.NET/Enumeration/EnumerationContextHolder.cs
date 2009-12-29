using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public class EnumerationContextHolder
   {
      private readonly IEnumerator<object> _enumerator;

      public EnumerationContextHolder(IEnumerator<object> enumerator)
      {
         _enumerator = enumerator;
      }     

      public IEnumerator<object> Enumerator
      {
         get { return _enumerator; }
      }
   }
}