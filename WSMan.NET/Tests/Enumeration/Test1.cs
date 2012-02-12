using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using WSMan.NET.Enumeration.Faults;
using WSMan.NET.SOAP;

// ReSharper disable InconsistentNaming
namespace WSMan.NET.Enumeration
{
    [TestFixture]
    public class Test1
    {
        [Test]
        public void It_returns_proper_fault_if_filter_dialect_is_not_supported()
        {
            var enumerationServer = new EnumerationServer()
                .Bind("http://tempuri.org", "supportedDialect", typeof (object),
                      MockRepository.GenerateMock<IEnumerationRequestHandler>());

            var soapClient = new TestingSOAPClient(enumerationServer);
            var enumerationClient = new EnumerationClient(false, soapClient);

            try
            {
                enumerationClient
                    .EnumerateEPR("http://tempuri.org", new Filter("unsupportedDialect", "A"), 10)
                    .ToList();            
            }
            catch (FaultException ex)
            {
                Assert.AreEqual(new NotSupportedDialectFaultException(), ex);
                return;
            }
            Assert.Fail();
        }
    }
}
// ReSharper restore InconsistentNaming
