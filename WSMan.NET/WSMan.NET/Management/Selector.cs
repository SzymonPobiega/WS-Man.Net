using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WSMan.NET.Addressing;

namespace WSMan.NET.Management
{
    public sealed class Selector
    {
        private readonly string _name;
        private readonly string _simpleValue;
        private readonly EndpointReference _addressReferenceValue;

        public Selector(string name, string value)
            : this(name)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            _simpleValue = value;
        }

        public Selector(string name, EndpointReference value)
            : this(name)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            _addressReferenceValue = value;
        }

        private Selector(string name)
        {
            _name = name;
        }

        public Selector(XElement element)
        {
            _name = element.Attribute("Name").Value;
            if (element.FirstNode.NodeType == XmlNodeType.Text)
            {
                _simpleValue = ((XText) element.FirstNode).Value;
            }
            else if(element.FirstNode.NodeType == XmlNodeType.Element)
            {
                var reference = new EndpointReference();
                reference.ReadXml(element.FirstNode.CreateReader());
                _addressReferenceValue = reference;
            }
        }

        public XNode Write()
        {
            return new XElement(Const.Namespace + "Selector",
                                new XAttribute("Name", _name),
                                IsSimpleValue
                                    ? new XText(_simpleValue)
                                    : WriteEndpointReference()
                );
        }

        private XNode WriteEndpointReference()
        {
            var buffer= new StringBuilder();
            using (var writer = XmlWriter.Create(buffer))
            {
                _addressReferenceValue.WriteXml(writer);
                writer.Flush();
            }
            return XElement.Parse(buffer.ToString());
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get
            {
                if (SimpleValue != null)
                {
                    return SimpleValue;
                }
                return AddressReference;
            }
        }

        public bool IsSimpleValue
        {
            get { return _simpleValue != null; }
        }

        public bool IsAddressReference
        {
            get { return _addressReferenceValue != null; }
        }

        public string SimpleValue
        {
            get
            {
                if (!IsSimpleValue)
                {
                    throw new InvalidOperationException("This selector contains address reference value.");
                }
                return _simpleValue;
            }
        }

        public EndpointReference AddressReference
        {
            get
            {
                if (!IsAddressReference)
                {
                    throw new InvalidOperationException("This selctor contains simple value.");
                }
                return _addressReferenceValue;
            }
        }

        public override string ToString()
        {
            var value = IsSimpleValue ? _simpleValue : _addressReferenceValue.ToString();
            return string.Format("{0}: {1}", _name, value);
        }
    }
}