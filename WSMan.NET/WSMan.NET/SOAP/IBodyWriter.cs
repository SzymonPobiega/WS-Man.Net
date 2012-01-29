using System.Xml;

namespace WSMan.NET.SOAP
{
    public interface IBodyWriter
    {
        void OnWriteBodyContents(XmlWriter writer);
    }
}