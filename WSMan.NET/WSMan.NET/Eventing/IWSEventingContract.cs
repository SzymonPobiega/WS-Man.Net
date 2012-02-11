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
      [OperationContract(Action = Constants.SubscribeAction, ReplyAction = Constants.SubscribeResponseAction)]
      SubscribeResponse Subscribe(SubscribeRequest request);

      [OperationContract(Action = Constants.UnsubscribeAction, IsOneWay = true)]
      void Unsubscribe(UnsubscribeRequest request);

      [OperationContract(Action = Constants.RenewAction, ReplyAction = Constants.RenewResponseAction)]
      RenewResponse Renew(RenewRequest request);
   }
}
