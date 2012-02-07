using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration
{
    public sealed class TotalItemsCountEstimateHeader : IMessageHeader
    {
        private int _value;

        public TotalItemsCountEstimateHeader()
        {
        }

        public TotalItemsCountEstimateHeader(int value)
        {
            _value = value;
        }

        public XName Name
        {
            get { return Management.Const.Namespace + "TotalItemsCountEstimate"; }
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