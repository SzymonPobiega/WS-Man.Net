namespace WSMan.NET.SOAP
{
    public class IncomingHeaders : IIncomingHeaders
    {
        private readonly IncomingMessage _incomingMessage;

        public IncomingHeaders(IncomingMessage incomingMessage)
        {
            _incomingMessage = incomingMessage;
        }

        public T GetHeader<T>() where T : class, IMessageHeader, new()
        {
            return _incomingMessage.GetHeader<T>();
        }
    }
}