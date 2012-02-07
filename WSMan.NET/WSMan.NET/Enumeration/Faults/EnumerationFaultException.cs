using System.Xml;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration.Faults
{
    public abstract class EnumerationFaultException : FaultException
    {
        protected EnumerationFaultException(string reason, string subcode) 
            : base(Constants.FaultAction, reason, FaultCode.Receiver, new XmlQualifiedName(subcode, Constants.NamespaceName))
        {
        }
    }
}