using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace WSMan.NET.Enumeration
{
   public class FilterMapExtension : IExtension<OperationContext>
   {
      private readonly FilterMap _map;

      public static void Activate(FilterMap map)
      {
         OperationContext.Current.Extensions.Add(new FilterMapExtension(map));
      }

      public static Type GetDialectType(string dialect)
      {
         return OperationContext.Current.Extensions.Find<FilterMapExtension>()._map.GetFilterType(dialect);
      }

      public static Type GetEnumeratedObjectType(string dialect)
      {
         return OperationContext.Current.Extensions.Find<FilterMapExtension>()._map.GetEnumeratedObjectType(dialect);
      }

      private FilterMapExtension(FilterMap map)
      {
         _map = map;
      }

      public void Attach(OperationContext owner)
      {
      }

      public void Detach(OperationContext owner)
      {
      }
   }
}