using System.Xml;

namespace WSMan.NET.WCF
{
    public interface IBodyWriter
    {
        void OnWriteBodyContents(XmlWriter writer);
    }
}