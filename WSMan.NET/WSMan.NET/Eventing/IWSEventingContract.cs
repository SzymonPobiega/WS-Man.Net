using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WSMan.NET.Eventing
{
   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
   public interface IWSEventingContract
   {
      [OperationContract(Action = Const.SubscribeAction, ReplyAction = Const.SubscribeResponseAction)]
      SubscribeResponse Subscribe(SubscribeRequest request);

      [OperationContract(Action = Const.UnsubscribeAction, IsOneWay = true)]
      void Unsubscribe(UnsubscribeRequest request);      
   }
}
