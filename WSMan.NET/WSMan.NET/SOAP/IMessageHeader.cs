using System.Collections.Generic;
using System.Xml.Linq;

namespace WSMan.NET.SOAP
{
    public interface IMessageHeader
    {
        XName Name { get; }
        IEnumerable<XNode> Write();
        void Read(IEnumerable<XNode> content);
    }
}