using System;
using WSMan.NET.Addressing;

namespace WSMan.NET.Transfer
{
    public delegate object ExtractBodyDelegate(Type expectedType);

    public interface ITransferRequestHandler
    {
        object HandleGet(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders);
        object HandlePut(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback);
        EndpointReference HandleCreate(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders, ExtractBodyDelegate extractBodyCallback);
        void HandlerDelete(IIncomingHeaders incomingHeaders, IOutgoingHeaders outgoingHeaders);
    }
}