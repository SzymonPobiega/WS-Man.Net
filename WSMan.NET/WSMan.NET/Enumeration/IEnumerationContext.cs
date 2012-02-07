using System.Collections.Generic;
using WSMan.NET.Management;

namespace WSMan.NET.Enumeration
{
    public interface IEnumerationContext
    {
        string Context { get; }
        object Filter { get; }
        IEnumerable<Selector> Selectors { get; }
    }
}