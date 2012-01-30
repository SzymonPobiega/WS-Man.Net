using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Management
{
    public class MaxEnvelopeSizeHeader : IMessageHeader
    {
        private int _value;

        public MaxEnvelopeSizeHeader()
        {
        }

        public MaxEnvelopeSizeHeader(int value)
        {
            _value = value;
        }

        public XName Name
        {
            get { return Const.Namespace + "MaxEnvelopeSize"; }
        }

        public int Value
        {
            get { return _value; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(Value.ToString());
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _value = int.Parse(text.Value);
        }

    }
}