using System.Linq;
using System.ServiceModel;
using System.Text;

namespace WSMan.NET.Enumeration
{
   [ServiceContract]
   [XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal)]
   public interface IWSEnumerationContract
   {
      [OperationContract(Action = Const.EnumerateAction, ReplyAction = Const.EnumerateResponseAction)]
      EnumerateResponse Enumerate(EnumerateRequest request);

      [OperationContract(Action = Const.PullAction, ReplyAction = Const.PullResponseAction)]
      PullResponse Pull(PullRequest request);
   }
}
