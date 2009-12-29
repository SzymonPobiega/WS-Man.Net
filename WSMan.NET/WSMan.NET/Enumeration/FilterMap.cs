using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public class FilterMap
   {
      public const string DefaultDialect = @"http://www.w3.org/TR/1999/REC-xpath-19991116";

      private readonly Dictionary<string, Type> _mapping = new Dictionary<string, Type>();

      public void Bind(string dialect, Type filterObjectType)
      {
         _mapping[dialect] = filterObjectType;         
      }                 

      public Type MapDialect(string dialect)
      {
         Type type;
         if (_mapping.TryGetValue(dialect, out type))
         {
            return type;
         }
         return null;         
      }      
   }
}