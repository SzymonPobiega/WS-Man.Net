namespace WSMan.NET.SOAP
{
    public interface IOutgoingHeaders
    {
        void AddHeader(IMessageHeader header, bool mustUnderstand);
    }
}