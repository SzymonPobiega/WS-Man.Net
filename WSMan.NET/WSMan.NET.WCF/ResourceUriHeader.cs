using System.ServiceModel.Channels;
using System.Xml;

namespace WSMan.NET.WCF
{
   public class ResourceUriHeader : AddressHeader
   {
      private const string ElementName = "ResourceURI";

      private readonly string _resourceUri;

      public ResourceUriHeader(string resourceUri)
      {
         _resourceUri = resourceUri;
      }

      public static ResourceUriHeader GetFrom(AddressHeaderCollection headerCollection)
      {
          return (ResourceUriHeader)headerCollection.FindHeader(ElementName, Const.ManagementNamespace);
      }

      public static ResourceUriHeader ReadFrom(XmlDictionaryReader reader)
      {
          reader.ReadStartElement(ElementName, Const.ManagementNamespace);
         string result = reader.Value;
         reader.Read();
         reader.ReadEndElement();
         return new ResourceUriHeader(result);
      }

      public static ResourceUriHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static ResourceUriHeader ReadFrom(MessageHeaders messageHeaders)
      {
         ResourceUriHeader result;
         int index = messageHeaders.FindHeader(ElementName, Const.ManagementNamespace);
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
         writer.WriteValue(ResourceUri);
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
          get { return Const.ManagementNamespace; }
      }

      public string ResourceUri
      {
         get { return _resourceUri; }
      }
   }
}