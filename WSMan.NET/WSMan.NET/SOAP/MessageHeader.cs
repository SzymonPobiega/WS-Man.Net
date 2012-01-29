using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using WSMan.NET.Transfer;

namespace WSMan.NET.SOAP
{
    public class MessageHeader
    {
        private readonly XName _name;
        private readonly IEnumerable<XNode> _content;
        private readonly bool _mustUnderstand;

        public MessageHeader(XElement headerElement)
        {
            _name = headerElement.Name;
            _content = headerElement.Nodes();
            var mustUnderstandAttribute = headerElement.Attribute(Constants.EnvelopeNamespace + "mustUnderstand");
            if (mustUnderstandAttribute!= null)
            {
                _mustUnderstand = XmlConvert.ToBoolean(mustUnderstandAttribute.Value);                
            }
        }

        public MessageHeader(XName name, IEnumerable<XNode> content, bool mustUnderstand)
        {
            _name = name;
            _mustUnderstand = mustUnderstand;
            _content = content;
        }

        public bool MustUnderstand
        {
            get { return _mustUnderstand; }
        }

        public IEnumerable<XNode> Content
        {
            get { return _content; }
        }

        public XName Name
        {
            get { return _name; }
        }

        public void Write(XmlWriter output)
        {
            output.WriteStartElement(_name);
            output.WriteAttributeString("mustUnderstand",Constants.EnvelopeNamespace.NamespaceName, XmlConvert.ToString(_mustUnderstand));
            foreach (var node in _content)
            {
                node.WriteTo(output);
            }
            output.WriteEndElement();
        }
    }
}