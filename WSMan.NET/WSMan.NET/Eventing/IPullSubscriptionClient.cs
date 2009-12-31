using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public interface IPullSubscriptionClient : IDisposable
   {
      IEnumerable<object> Pull();
   }
}