using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Management
{
    public sealed class FragmentTransferHeader : IMessageHeader
    {
        private string _expression;

        public FragmentTransferHeader()
        {
        }

        public FragmentTransferHeader(string expression)
        {
            _expression = expression;
        }

        public XName Name
        {
            get { return Const.Namespace + "FragmentTransfer"; }
        }

        public string Expression
        {
            get { return _expression; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(Expression);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _expression = text.Value;
        }
    }
}