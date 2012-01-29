using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public interface IOutgoingHeaders
    {
        void AddHeader(IMessageHeader header, bool mustUnderstand);
    }
}