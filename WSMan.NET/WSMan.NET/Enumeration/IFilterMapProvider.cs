using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public interface IFilterMapProvider
   {
      FilterMap ProvideFilterMap();
   }
}