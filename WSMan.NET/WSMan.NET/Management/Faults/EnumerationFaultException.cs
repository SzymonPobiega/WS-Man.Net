using System.Xml;
using WSMan.NET.SOAP;

namespace WSMan.NET.Management.Faults
{
    public abstract class ManagementFaultException : FaultException
    {
        protected ManagementFaultException(string reason, string subcode) 
            : base(Constants.FaultAction, reason, FaultCode.Receiver, new XmlQualifiedName(subcode, Constants.NamespaceName))
        {
        }
    }
}