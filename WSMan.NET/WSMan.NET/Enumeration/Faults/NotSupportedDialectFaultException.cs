namespace WSMan.NET.Enumeration.Faults
{
    public class NotSupportedDialectFaultException : EnumerationFaultException
    {
        public NotSupportedDialectFaultException()
            : base("The requested filtering dialect is not supported", "FilterDialectRequestedUnavailable")
        {
        }
    }
}