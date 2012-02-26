namespace WSMan.NET.Addressing.Faults
{
    public class DestinationUnreachableFaultException : AddressingFaultException
    {
        public DestinationUnreachableFaultException()
            : base("No route can be determined to reach the destination role defined by the Addressing To header.", "DestinationUnreachable")
        {
        }
    }
}