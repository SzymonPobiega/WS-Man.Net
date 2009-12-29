using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public class Const
   {
      public const string Namespace = @"http://schemas.xmlsoap.org/ws/2004/09/eventing";

      public const string SubscribeAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/Subscribe";
      public const string SubscribeResponseAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/SubscribeResponse";

      public const string UnsubscribeAction = @"http://schemas.xmlsoap.org/ws/2004/08/eventing/Unsubscribe";

      public const string DeliveryModePull = @"http://schemas.dmtf.org/wbem/wsman/1/wsman/Pull";
   }
}