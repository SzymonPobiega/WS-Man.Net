using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Addressing
{
    public class ActionHeader : IMessageHeader
    {
        private string _action;

        public ActionHeader()
        {
        }

        public ActionHeader(string action)
        {
            _action = action;
        }

        public XName Name
        {
            get { return Constants.Namespace + "Action"; }
        }

        public string Action
        {
            get { return _action; }
        }

        public IEnumerable<XNode> Write()
        {
            yield return new XText(Action);
        }

        public void Read(IEnumerable<XNode> content)
        {
            var text = (XText)content.Single();
            _action = text.Value;
        }
    }
}