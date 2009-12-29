using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace WSMan.NET.Eventing
{
   //[XmlType(Namespace = "http://schemas.xmlsoap.org/ws/2004/08/eventing")]
   //[XmlRoot("NotifyTo", Namespace = "http://schemas.xmlsoap.org/ws/2004/08/eventing", IsNullable = false)]
   public class EndpointReference
   {
      private EndpointAddress _address;

      public EndpointReference()
      {
      }

      public EndpointReference(EndpointAddress address)
      {
         _address = address;
      }

      public EndpointReference(EndpointAddressBuilder addressBuilder)
      {
         _address = addressBuilder.ToEndpointAddress();
      }

      #region IXmlSerializable Members
      public System.Xml.Schema.XmlSchema GetSchema()
      {
         return null;
      }
      public void ReadXml(XmlReader reader)
      {
         XmlDictionaryReader dictionaryReader = reader as XmlDictionaryReader;
         if (dictionaryReader == null)
         {
            throw new InvalidOperationException("Must be used with XmlDictionaryReader.");
         }
         _address = EndpointAddress.ReadFrom(dictionaryReader);
      }
      public void WriteXml(XmlWriter writer)
      {                  
         //_address.WriteContentsTo(
         //   OperationContext.Current .Addressing, 
         //   XmlDictionaryWriter.CreateDictionaryWriter(writer));
      }
      #endregion
   }
}