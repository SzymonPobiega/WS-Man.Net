using WSMan.NET.SOAP;

namespace WSMan.NET.Client
{
    public interface IMessageHeaderWithMustUnderstandSpecification
    {
        IMessageHeader Header { get; }
        bool MustUnderstand { get; }
    }
}