using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public interface IPullSubscriptionClient<T> : IDisposable
   {
      IEnumerable<T> Pull();
   }
}