using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Management;

namespace WSMan.NET.Enumeration
{
   public class EnumerationContext : IEnumerationContext
   {
      private readonly string _context;
      private readonly object _filter;
      private readonly IEnumerable<Selector> _selectors;

      public EnumerationContext(string context, object filter, IEnumerable<Selector> selectors)
      {
         _context = context;
         _selectors = selectors;
         _filter = filter;
      }

      public string Context
      {
         get { return _context; }
      }

      public object Filter
      {
         get { return _filter; }
      }

      public IEnumerable<Selector> Selectors
      {
         get { return _selectors; }
      }
   }
}