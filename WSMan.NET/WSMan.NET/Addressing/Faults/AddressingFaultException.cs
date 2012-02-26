using System.Xml;
using WSMan.NET.SOAP;

namespace WSMan.NET.Addressing.Faults
{
    public abstract class AddressingFaultException : FaultException
    {
        protected AddressingFaultException(string reason, string subcode) 
            : base(Constants.FaultAction, reason, FaultCode.Receiver, new XmlQualifiedName(subcode, Constants.Namespace.NamespaceName))
        {
        }
    }
}