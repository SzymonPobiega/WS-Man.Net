
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.SOAP;
using WSMan.NET.Transfer;
using Constants = WSMan.NET.SOAP.Constants;

// ReSharper disable InconsistentNaming
namespace WSMan.NET
{
    [TestFixture]
    public class MessageHeaderTests
    {
        [Test]
        public void It_reads_header_name()
        {
            var headerElement = new XElement("headerName");

            var header = new MessageHeader(headerElement);

            Assert.AreEqual("headerName",header.Name.LocalName);
        }

        [Test]
        public void It_reads_header_namespace()
        {
            var headerElement = new XElement(XName.Get("headerName","headerNamespace"));

            var header = new MessageHeader(headerElement);

            Assert.AreEqual("headerNamespace", header.Name.Namespace.NamespaceName);
        }

        [Test]
        public void It_assumes_must_understand_is_false_if_there_is_no_attribute()
        {
            var headerElement = new XElement("headerName");

            var header = new MessageHeader(headerElement);

            Assert.IsFalse(header.MustUnderstand);
        }
        
        [Test]
        public void It_reads_must_understand_from_attribute()
        {
            var headerElement = new XElement("headerName",
                new XAttribute(Constants.Namespace + "mustUnderstand", true));

            var header = new MessageHeader(headerElement);

            Assert.IsTrue(header.MustUnderstand);
        }

        [Test]
        public void It_writes_must_understand()
        {
            var header = new MessageHeader(XName.Get("headerName", "headerNamespace"), new XNode[] {}, true);

            var headerElement = XElementExtensions.ParseGeneratedXml(header.Write);

            Assert.AreEqual("true", headerElement.Attribute(Constants.Namespace + "mustUnderstand").Value);
        }

        [Test]
        public void It_writes_header_name_with_namespace()
        {
            var header = new MessageHeader(XName.Get("headerName", "headerNamespace"), new XNode[] { }, true);

            var headerElement = XElementExtensions.ParseGeneratedXml(header.Write);

            Assert.AreEqual("headerName", headerElement.Name.LocalName);
            Assert.AreEqual("headerNamespace", headerElement.Name.NamespaceName);
        }

        [Test]
        public void It_writes_header_contents()
        {
            var header = new MessageHeader(XName.Get("headerName", "headerNamespace"),
                                           new XNode[]
                                               {
                                                   new XText("someText"),
                                                   new XElement("someNestedElement")
                                               }, true);

            var headerElement = XElementExtensions.ParseGeneratedXml(header.Write);
            var headerContents = headerElement.Nodes().ToList();
            var textNode = (XText) headerContents[0];
            Assert.AreEqual("someText", textNode.Value);
            var nestedElement = (XElement) headerContents[1];
            Assert.AreEqual("someNestedElement", nestedElement.Name.LocalName);
        }
    }
}
// ReSharper restore InconsistentNaming
