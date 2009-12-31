using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace WSMan.NET.Eventing
{
   public class IdentifierHeader : AddressHeader
   {
      private const string ElementName = "Identifier";

      private readonly string _value;

      public IdentifierHeader(string value)
      {
         _value = value;
      }

      public static IdentifierHeader Unique()
      {
         return new IdentifierHeader(Guid.NewGuid().ToString());
      }

      public static IdentifierHeader GetFrom(AddressHeaderCollection headerCollection)
      {
         return (IdentifierHeader)headerCollection.FindHeader(ElementName, Const.Namespace);
      }

      public static IdentifierHeader ReadFrom(XmlDictionaryReader reader)
      {
         reader.ReadStartElement(ElementName, Const.Namespace);
         string result = reader.Value;
         reader.Read();
         reader.ReadEndElement();
         return new IdentifierHeader(result);
      }

      public static IdentifierHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static IdentifierHeader ReadFrom(MessageHeaders messageHeaders)
      {
         IdentifierHeader result;
         int index = messageHeaders.FindHeader(ElementName, Const.Namespace);
         if (index < 0)
         {
            return null;
         }
         using (XmlDictionaryReader readerAtHeader = messageHeaders.GetReaderAtHeader(index))
         {
            result = ReadFrom(readerAtHeader);
         }
         MessageHeaderInfo headerInfo = messageHeaders[index];
         if (!messageHeaders.UnderstoodHeaders.Contains(headerInfo))
         {
            messageHeaders.UnderstoodHeaders.Add(headerInfo);
         }
         return result;
      }

      protected override void OnWriteAddressHeaderContents(XmlDictionaryWriter writer)
      {
         writer.WriteValue(_value);
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return Const.Namespace; }
      }

      public string Value
      {
         get { return _value; }
      }
   }
}