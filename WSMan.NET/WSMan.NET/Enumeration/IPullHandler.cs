using System;

namespace WSMan.NET.Enumeration
{
    public interface IPullHandler : IDisposable
    {
        PullResult Pull(int? maxElements, TimeSpan? maxTime, string context);
    }
}