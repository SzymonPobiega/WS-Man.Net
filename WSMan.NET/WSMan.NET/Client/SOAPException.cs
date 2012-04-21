using System;
using System.Runtime.Serialization;

namespace WSMan.NET.Client
{
    [Serializable]
    public class SOAPException : Exception
    {
        public SOAPException()
        {
        }

        public SOAPException(string message) : base(message)
        {
        }

        public SOAPException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SOAPException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}