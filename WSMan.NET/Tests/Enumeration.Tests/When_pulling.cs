using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace WSMan.NET.Enumeration.Tests
{
   [TestFixture]
   public class When_pulling
   {
      [Test]
      public void Additional_data_in_response_is_ignored()
      {
         const string xml = @"<ns5:PullResponse xmlns:ns3=""http://schemas.xmlsoap.org/ws/2004/08/addressing""
    xmlns:ns4=""http://schemas.xmlsoap.org/ws/2004/08/eventing""
    xmlns:ns5=""http://schemas.xmlsoap.org/ws/2004/09/enumeration""
    xmlns:ns6=""http://schemas.xmlsoap.org/ws/2004/09/transfer""
    xmlns:ns7=""http://schemas.dmtf.org/wbem/wsman/1/wsman.xsd""
    xmlns:ns8=""http://schemas.dmtf.org/wbem/wsman/identity/1/wsmanidentity.xsd""
    xmlns:ns9=""http://jsr262.dev.java.net/jmxconnector"">
      <ns5:Items>
        <ns3:EndpointReference>
          <ns3:Address>http://iblongsw573721:10003/</ns3:Address>
          <ns3:ReferenceParameters>
            <ns7:ResourceURI>
            http://jsr262.dev.java.net/DynamicMBeanResource</ns7:ResourceURI>
            <ns7:SelectorSet>
              <ns7:Selector Name=""ObjectName"">
              JMImplementation:type=MBeanServerDelegate</ns7:Selector>
            </ns7:SelectorSet>
          </ns3:ReferenceParameters>
        </ns3:EndpointReference>
      </ns5:Items>
      <ns5:EndOfSequence xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xsi:type=""xs:string""></ns5:EndOfSequence>
    </ns5:PullResponse>";

         AddressingVersionExtension.Activate(AddressingVersion.WSAddressingAugust2004);
         EnumerationModeExtension.Activate(EnumerationMode.EnumerateEPR, null);
         XmlSerializer xs = new XmlSerializer(typeof(PullResponse), null, new Type[]{typeof(string)}, new XmlRootAttribute("PullResponse") { Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration" }, null);         
         object deserializedObject = xs.Deserialize(new StringReader(xml));
      }

      [Test]
      [ExpectedException(typeof(FaultException))]
      public void If_invalid_subscription_context_specified_exception_is_thrown()
      {
         EnumerationServer server = new EnumerationServer();
         PullRequest request = new PullRequest
                                  {
                                     EnumerationContext = new EnumerationContextKey("aaa")
                                  };
         server.Pull(request);
      }      
   }
}