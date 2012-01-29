namespace WSMan.NET.Transfer
{
   public class Constants
   {
      public const string Namespace = "http://schemas.xmlsoap.org/ws/2004/09/transfer";
      public const string GetAction = Namespace +"/Get";
      public const string GetResponseAction = Namespace + "/GetResponse";
      public const string PutAction = Namespace + "/Put";
      public const string PutResponseAction = Namespace + "/PutResponse";
      public const string CreateAction = Namespace + "/Create";
      public const string CreateResponseAction = Namespace + "/CreateResponse";
      public const string DeleteAction = Namespace + "/Delete";
      public const string DeleteResponseAction = Namespace + "/DeleteResponse";

      public const string CreateResponse_ResourceCreatedElement = "ResourceCreated";
   }
}