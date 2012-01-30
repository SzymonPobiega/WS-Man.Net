using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Management
{
    public sealed class SelectorSetHeader : IMessageHeader
    {
        internal const string ElementName = "SelectorSet";
        private readonly List<Selector> _selectors;

        public XName Name
        {
            get { return Const.Namespace + "SelectorSet"; }
        }

        public IEnumerable<XNode> Write()
        {
            return Selectors.Select(x => x.Write());
        }

        public void Read(IEnumerable<XNode> content)
        {
            _selectors.AddRange(content.Cast<XElement>().Select(x => new Selector(x)));
        }

        public List<Selector> Selectors
        {
            get { return _selectors; }
        }

        public SelectorSetHeader()
        {
            _selectors = new List<Selector>();
        }

        public SelectorSetHeader(params Selector[] selectors)
        {
            _selectors = new List<Selector>(selectors);
        }

        public SelectorSetHeader(IEnumerable<Selector> selectors)
        {
            _selectors = new List<Selector>(selectors);
        }
    }
}