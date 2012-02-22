namespace WSMan.NET.SOAP
{
    public interface IIncomingHeaders
    {
        T GetHeader<T>() where T : class, IMessageHeader, new();
    }
}