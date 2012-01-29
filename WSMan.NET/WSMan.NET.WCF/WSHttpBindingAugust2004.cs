using System.ServiceModel;
using System.ServiceModel.Channels;

namespace WSMan.NET.WCF
{
    public class BasicHttpBindingWithAddressing : BasicHttpBinding
    {
        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = base.CreateBindingElements();
            var textEncoding = elements.Find<TextMessageEncodingBindingElement>();
            textEncoding.MessageVersion = MessageVersion.Soap12WSAddressingAugust2004;
            return elements;
        }
    }

    public class WSHttpBindingAugust2004 : WSHttpBinding
    {
        public WSHttpBindingAugust2004(SecurityMode securityMode)
            : base(securityMode)
        {
        }

        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = base.CreateBindingElements();
            TextMessageEncodingBindingElement textEncoding = elements.Find<TextMessageEncodingBindingElement>();
            textEncoding.MessageVersion = MessageVersion.Soap12WSAddressingAugust2004;
            return elements;
        }
    }
}
