using System;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing
{
   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
   public interface IWSEventingPullDeliveryContract
   {
      [OperationContract(Action = Enumeration.Const.PullAction, ReplyAction = Enumeration.Const.PullResponseAction)]
      PullResponse Pull(PullRequest request);
   }
}
