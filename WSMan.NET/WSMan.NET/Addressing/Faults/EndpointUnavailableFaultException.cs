namespace WSMan.NET.Addressing.Faults
{
    public class EndpointUnavailableFaultException : AddressingFaultException
    {
        public EndpointUnavailableFaultException()
            : base("The specified endpoint is currently unavailable.", "EndpointUnavailable")
        {
        }
    }
}