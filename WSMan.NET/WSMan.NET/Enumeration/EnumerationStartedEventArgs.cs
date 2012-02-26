using System;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public class EnumerationStartedEventArgs : EventArgs
   {
      private readonly IEnumerator<object> _enumerator;
      private readonly EnumerationMode _mode;

      public EnumerationStartedEventArgs(IEnumerator<object> enumerator, EnumerationMode mode)
      {
         _enumerator = enumerator;
         _mode = mode;
      }

      public IEnumerator<object> Enumerator
      {
         get { return _enumerator; }
      }

      public EnumerationMode Mode
      {
         get { return _mode; }
      }
   }
}