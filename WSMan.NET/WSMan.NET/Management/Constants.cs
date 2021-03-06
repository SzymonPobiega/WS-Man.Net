using System.Xml.Linq;

namespace WSMan.NET.Management
{
    public static class Constants
    {
        public static readonly XNamespace Namespace = NamespaceName;
        public const string NamespaceName = @"http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd";
        public const string FaultAction = @"http://schemas.dmtf.org/wbem/wsman/1/wsman/fault";
    }
}