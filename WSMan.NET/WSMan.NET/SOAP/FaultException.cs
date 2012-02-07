using System;
using System.Xml;

namespace WSMan.NET.SOAP
{
    public class FaultException : Exception
    {
        private readonly string _action;
        private readonly string _reason;
        private readonly XmlQualifiedName _code;
        private readonly XmlQualifiedName[] _subcodes;

        public FaultException(string action, string reason, XmlQualifiedName code, params XmlQualifiedName[] subcodes)
        {
            _action = action;
            _reason = reason;
            _code = code;
            _subcodes = subcodes;
        }

        public OutgoingMessage CreateMessage()
        {
            return FaultMessage.Create(_action, _reason, _code, _subcodes);
        }
    }
}