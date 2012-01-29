using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public interface IIncomingHeaders
    {
        T GetHeader<T>() where T : class, IMessageHeader, new();
    }
}