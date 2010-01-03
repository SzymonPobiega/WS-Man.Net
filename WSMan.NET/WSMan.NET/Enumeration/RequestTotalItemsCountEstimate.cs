using System.ServiceModel.Channels;
using System.Xml;

namespace WSMan.NET.Enumeration
{
   public class RequestTotalItemsCountEstimate : MessageHeader
   {
      private const string ElementName = "RequestTotalItemsCountEstimate";

      public static bool IsPresent
      {
         get
         {
            RequestTotalItemsCountEstimate header =
               OperationContextProxy.Current.FindHeader<RequestTotalItemsCountEstimate>();
            return header != null;
         }      
      }

      public static RequestTotalItemsCountEstimate ReadFrom(MessageHeaders messageHeaders)
      {
         int index = messageHeaders.FindHeader(ElementName, Const.Namespace);
         if (index < 0)
         {
            return null;
         }         
         MessageHeaderInfo headerInfo = messageHeaders[index];
         if (!messageHeaders.UnderstoodHeaders.Contains(headerInfo))
         {
            messageHeaders.UnderstoodHeaders.Add(headerInfo);
         }
         return new RequestTotalItemsCountEstimate();
      }

      public override string Name
      {
         get { return ElementName; }
      }

      public override string Namespace
      {
         get { return Const.Namespace; }
      }

      protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
      {         
      }
   }
}