using System;
using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public interface IMessageHeaderWithMustUnderstandSpecification
    {
        IMessageHeader Header { get; }
        bool MustUnderstand { get; }
    }
}