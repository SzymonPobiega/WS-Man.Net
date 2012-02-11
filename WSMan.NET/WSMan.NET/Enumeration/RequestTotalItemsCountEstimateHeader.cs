using System.Collections.Generic;
using System.Xml.Linq;
using WSMan.NET.SOAP;

namespace WSMan.NET.Enumeration
{
    public class RequestTotalItemsCountEstimateHeader : IMessageHeader
    {
        public IEnumerable<XNode> Write()
        {
            yield break;
        }

        public void Read(IEnumerable<XNode> content)
        {
        }

        public XName Name
        {
            get { return Management.Constants.Namespace + "RequestTotalItemsCountEstimate"; }
        }
    }
}