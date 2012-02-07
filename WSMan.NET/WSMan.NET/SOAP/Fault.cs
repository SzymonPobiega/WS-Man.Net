using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WSMan.NET.SOAP
{
    [XmlRoot(ElementName = "Fault", Namespace = Constants.NamespaceName)]
    public class Fault
    {
        [XmlElement("Code", Namespace = Constants.NamespaceName)]
        public FaultCode Code { get; set; }

        [XmlElement("Reason", Namespace = Constants.NamespaceName)]
        public FaultReason Reason { get; set; }

        [XmlElement("Detail", Namespace = Constants.NamespaceName)]
        public FaultDetail Detail { get; set; }
    }

    public class FaultDetail
    {
        [XmlAnyElement]
        public XmlNode[] Value { get; set; }
    }

    public class FaultReason
    {
        [XmlElement(ElementName = "Text", Namespace = Constants.NamespaceName)]
        public List<FaultReasonText> TextVersions { get; set; }
    }

    public class FaultReasonText
    {
        [XmlAttribute("xml:lang")]
        public string LanguageCode { get; set; }

        [XmlText]
        public string Value { get; set; }
    }


    public class FaultCode
    {
        public static readonly XmlQualifiedName DataEncodingUnknown = new XmlQualifiedName("DataEncodingUnknown", Constants.NamespaceName);
        public static readonly XmlQualifiedName MustUnderstand = new XmlQualifiedName("MustUnderstand", Constants.NamespaceName);
        public static readonly XmlQualifiedName Receiver = new XmlQualifiedName("Receiver", Constants.NamespaceName);
        public static readonly XmlQualifiedName Sender = new XmlQualifiedName("Sender", Constants.NamespaceName);
        public static readonly XmlQualifiedName VersionMismatch = new XmlQualifiedName("VersionMismatch", Constants.NamespaceName);

        [XmlElement(ElementName = "Value", Namespace = Constants.NamespaceName)]
        public XmlQualifiedName Value { get; set; }

        [XmlElement(ElementName = "Subcode", Namespace = Constants.NamespaceName)]
        public FaultSubcode Subcode { get; set; }
    }

    public class FaultSubcode
    {
        [XmlElement(ElementName = "Value", Namespace = Constants.NamespaceName)]
        public XmlQualifiedName Value { get; set; }

        [XmlElement(ElementName = "Subcode", Namespace = Constants.NamespaceName)]
        public FaultSubcode Subcode { get; set; }
    }
}