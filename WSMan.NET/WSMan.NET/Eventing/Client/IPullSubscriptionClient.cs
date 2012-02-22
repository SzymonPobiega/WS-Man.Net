using System;
using System.Collections.Generic;

namespace WSMan.NET.Eventing.Client
{
   public interface IPullSubscriptionClient<T> : IDisposable
   {
      IEnumerable<T> Pull();
      IEnumerable<T> PullOnce();
   }
}