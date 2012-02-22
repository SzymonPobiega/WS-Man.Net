namespace WSMan.NET.SOAP
{
    public class OutgoingHeaders : IOutgoingHeaders
    {
        private readonly OutgoingMessage _outgoingMessage;

        public OutgoingHeaders(OutgoingMessage outgoingMessage)
        {
            _outgoingMessage = outgoingMessage;
        }

        public void AddHeader(IMessageHeader header, bool mustUnderstand)
        {
            _outgoingMessage.AddHeader(header, mustUnderstand);
        }
    }
}