using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using WSMan.NET.Addressing;
using WSMan.NET.Transfer;

namespace WSMan.NET.SOAP
{
    public static class FaultMessage
    {
        public static bool IsFault(this IncomingMessage message)
        {
            var bodyReader = message.GetReaderAtBodyContents();
            return bodyReader.Name == "Fault"
                   && bodyReader.NamespaceURI == Constants.NamespaceName;
        }

        public static FaultException CreateFaultException(this IncomingMessage faultMessage)
        {
            if (!faultMessage.IsFault())
            {
                throw new InvalidOperationException("Message is not a fault.");
            }
            var actionHeader = faultMessage.GetHeader<ActionHeader>();
            var xs = new XmlSerializer(typeof(Fault));
            var body = (Fault)xs.Deserialize(faultMessage.GetReaderAtBodyContents());
            return new FaultException(actionHeader.Action,
                body.Reason.TextVersions.First().Value,
                body.Code.Value, 
                ExtractSubcodes(body.Code.Subcode));
        }

        private static IEnumerable<XmlQualifiedName> ExtractSubcodes(FaultSubcode rootSubcode)
        {
            if (rootSubcode == null)
            {
                yield break;
            }
            yield return rootSubcode.Value;
            foreach (var subcode in ExtractSubcodes(rootSubcode.Subcode))
            {
                yield return subcode;
            }
        }

        public static OutgoingMessage Create(string action, string reason, XmlQualifiedName code, IEnumerable<XmlQualifiedName> subcodes)
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