using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace WSMan.NET.SOAP
{
    public class FaultException : Exception
    {
        private readonly string _action;
        private readonly string _reason;
        private readonly XmlQualifiedName _code;
        private readonly List<XmlQualifiedName> _subcodes;

        public FaultException(string action, string reason, XmlQualifiedName code, IEnumerable<XmlQualifiedName> subcodes)
        {
            _action = action;
            _reason = reason;
            _code = code;
            _subcodes = subcodes.ToList();
        }

        public FaultException(string action, string reason, XmlQualifiedName code, params XmlQualifiedName[] subcodes)
            : this(action, reason, code, ((IEnumerable<XmlQualifiedName>)subcodes))
        {
        }

        public string Action
        {
            get { return _action; }
        }

        public string Reason
        {
            get { return _reason; }
        }

        public XmlQualifiedName Code
        {
            get { return _code; }
        }

        public IEnumerable<XmlQualifiedName> Subcodes
        {
            get { return _subcodes; }
        }

        public OutgoingMessage CreateMessage()
        {
            return FaultMessage.Create(Action, Reason, Code, _subcodes);
        }

        public override bool Equals(object obj)
        {
            var other = obj as FaultException;
            return other != null
                   && Action == other.Action
                   && Reason == other.Reason
                   && Code == other.Code
                   && Subcodes.SequenceEqual(other.Subcodes);
        }
    }
}