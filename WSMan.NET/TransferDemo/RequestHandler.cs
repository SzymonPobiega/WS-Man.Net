using WSMan.NET.Addressing;
using WSMan.NET.Transfer;

namespace TransferDemo
{
    public class RequestHandler : ITransferRequestHandler
    {
        private SampleData _data;

        public object HandleGet(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            return _data;
        }

        public object HandlePut(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback)
        {
            _data = (SampleData)extractBodyCallback(typeof (SampleData));
            return _data;
        }

        public EndpointReference HandleCreate(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback)
        {
            _data = (SampleData)extractBodyCallback(typeof(SampleData));
            return new EndpointReference("http://example.com");
        }

        public void HandlerDelete(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders)
        {
            _data = null;
        }
    }
}