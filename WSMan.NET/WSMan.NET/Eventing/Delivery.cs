using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Eventing
{
    [XmlType(Namespace = Constants.NamespaceName)]
    public class Delivery
    {
        public const string DeliveryModePull = @"http://schemas.dmtf.org/wbem/wsman/1/wsman/Pull";

        public static Delivery Pull()
        {
            return new Delivery { Mode = DeliveryModePull };
        }

        [XmlAttribute(DataType = "anyURI")]
        public string Mode { get; set; }

        [XmlAnyElement]
        public XmlNode[] Any { get; set; }

        [XmlAnyAttribute]
        public XmlAttribute[] AnyAttr { get; set; }

    }
}