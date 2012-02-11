namespace WSMan.NET.Management.Faults
{
    public class TimedOutFaultException : ManagementFaultException
    {
        public TimedOutFaultException()
            : base("The operation has timed out.", "TimedOut")
        {
        }
    }
}