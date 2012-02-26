using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public sealed class Mandatory : IMessageHeaderWithMustUnderstandSpecification
    {
        private readonly IMessageHeader _header;

        public Mandatory(IMessageHeader header)
        {
            _header = header;
        }

        public IMessageHeader Header
        {
            get { return _header; }
        }

        public bool MustUnderstand
        {
            get { return true; }
        }
    }
}