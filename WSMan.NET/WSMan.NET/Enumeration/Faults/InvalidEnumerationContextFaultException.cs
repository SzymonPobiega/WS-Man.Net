namespace WSMan.NET.Enumeration.Faults
{
    public class InvalidEnumerationContextFaultException : EnumerationFaultException
    {
        public InvalidEnumerationContextFaultException()
            : base("The supplied enumeration context is invalid.", "InvalidEnumerationContext")
        {
        }
    }
}