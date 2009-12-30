using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public class EnumerationContext : IEnumerationContext
   {
      private readonly string _context;
      private readonly Filter _filter;

      public EnumerationContext(string context, Filter filter)
      {
         _context = context;
         _filter = filter;
      }

      public string Context
      {
         get { return _context; }
      }

      public Filter Filter
      {
         get { return _filter; }
      }

   }
}