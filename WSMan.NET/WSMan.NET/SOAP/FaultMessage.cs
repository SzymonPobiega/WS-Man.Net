using System.Collections.Generic;
using System.Linq;
using System.Xml;
using WSMan.NET.Addressing;
using WSMan.NET.Transfer;

namespace WSMan.NET.SOAP
{
    public static class FaultMessage
    {
        public static OutgoingMessage Create(string action, string reason, XmlQualifiedName code, params XmlQualifiedName[] subcodes)
        {
            var message = new OutgoingMessage();
            message.AddHeader(new ActionHeader(action), false);
            var fault = new Fault
                            {
                                Code = CreateCode(code, subcodes),
                                Detail = new FaultDetail(),
                                Reason = CreateReason(reason)
                            };
            message.SetBody(new SerializerBodyWriter(fault));
            return message;
        }

        private static FaultCode CreateCode(XmlQualifiedName code, IEnumerable<XmlQualifiedName> subcodes)
        {
            return new FaultCode
                       {
                           Value = code,
                           Subcode = CreateSubcodes(subcodes)
                       };
        }

        private static FaultSubcode CreateSubcodes(IEnumerable<XmlQualifiedName> subcodes)
        {
            FaultSubcode last = null;
            return subcodes.Reverse().Aggregate(last, (prev, code) => new FaultSubcode { Value = code, Subcode = prev });
        }

        private static FaultReason CreateReason(string reason)
        {
            return new FaultReason
                       {
                           TextVersions = new List<FaultReasonText>
                                              {
                                                  new FaultReasonText
                                                      {
                                                          LanguageCode = "en",
                                                          Value = reason
                                                      }
                                              }
                       };
        }
    }
}