using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WSMan.NET.Eventing
{
   [ServiceContract]
   public interface IWSEventingContract
   {
      [OperationContract(Action = Const.SubscribeAction, ReplyAction = Const.SubscribeResponseAction)]
      SubscribeResponse Subscribe(SubscribeRequest request);

      [OperationContract(Action = Const.SubscribeAction, ReplyAction = Const.SubscribeResponseAction, IsOneWay = true)]
      void Unsubscribe(UnsubscribeRequest request);

      //[OperationContract(Action = Enumeration.Const.PullAction, ReplyAction = Enumeration.Const.PullResponseAction)]
      //PullResponse Pull(PullRequest request);
   }
}
