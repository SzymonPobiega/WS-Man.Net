using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET.WCF
{
    public class ManagementWSTransferFaultHandler : IWSTransferFaultHandler
    {
        public Exception HandleFault(Message faultMessage)
        {
            return new FaultException(MessageFault.CreateFault(faultMessage, int.MaxValue));
        }
    }
}