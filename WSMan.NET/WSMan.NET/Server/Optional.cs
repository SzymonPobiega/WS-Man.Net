using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public sealed class Optional<T> : IMessageHeaderWithMustUnderstandSpecification
        where T : IMessageHeader
    {
        private readonly T _header;

        public Optional(T header)
        {
            _header = header;
        }

        public IMessageHeader Header
        {
            get { return _header; }
        }

        public bool MustUnderstand
        {
            get { return false; }
        }

        public static implicit operator Optional<T>(T header)
        {
            return new Optional<T>(header);
        }
    }
}