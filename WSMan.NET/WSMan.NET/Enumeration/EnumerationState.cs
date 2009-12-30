using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public class EnumerationState
   {
      private readonly IEnumerator<object> _enumerator;
      
      public EnumerationState(IEnumerator<object> enumerator)
      {
         _enumerator = enumerator;
      }

      public IEnumerator<object> Enumerator
      {
         get { return _enumerator; }
      }
   }
}