using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;
using WSMan.NET.SOAP;

// ReSharper disable InconsistentNaming
// ReSharper disable PossibleNullReferenceException
namespace WSMan.NET
{
    [TestFixture]
    public class FaultMessageTests
    {
        [Test]
        public void It_sets_fault_action()
        {
            var message = FaultMessage.Create("someAction", "no reason", FaultCode.Sender);

            var envelope = XElementExtensions.ParseGeneratedXml(message.Write);

            var headerSectionElement = envelope.Element(Constants.Header);
            var actionHeader = headerSectionElement.Element(Addressing.Constants.Namespace + "Action");
            var actionValue = ((XText)actionHeader.FirstNode).Value;
            Assert.AreEqual("someAction", actionValue);
        }

        [Test]
        public void It_sets_fault_code_with_nested_subcodes()
        {
            var message = FaultMessage.Create("someAction", "no reason", FaultCode.Sender, FaultCode.Receiver, FaultCode.VersionMismatch);

            var envelope = XElementExtensions.ParseGeneratedXml(message.Write);

            var bodyElement = envelope.Element(Constants.Body);
            var codeElement = bodyElement
                .Element(Constants.Namespace + "Fault")
                .Element(Constants.Namespace + "Code");                

            var rootSubcodeElement = codeElement.Element(Constants.Namespace + "Subcode");
            var childSubcodeElement = rootSubcodeElement.Element(Constants.Namespace + "Subcode");

            Assert.AreEqual("Sender", ExtractCodeValue(codeElement));
            Assert.AreEqual("Receiver", ExtractCodeValue(rootSubcodeElement));
            Assert.AreEqual("VersionMismatch", ExtractCodeValue(childSubcodeElement));
        }

        private static string ExtractCodeValue(XElement codeElement)
        {
            return ((XText)codeElement.Element(Constants.Namespace + "Value").FirstNode).Value;
        }
    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore PossibleNullReferenceException
