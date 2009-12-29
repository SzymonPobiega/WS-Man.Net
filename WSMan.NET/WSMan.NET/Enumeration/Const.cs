using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
   public class Const
   {
      public const string Namespace = "http://schemas.xmlsoap.org/ws/2004/09/enumeration";

      public const string WSAddressing200408Namespace = @"http://schemas.xmlsoap.org/ws/2004/08/addressing";

      public const string EnumerateAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/Enumerate";
      public const string EnumerateResponseAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/EnumerateResponse";
      public const string PullAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/Pull";
      public const string PullResponseAction = @"http://schemas.xmlsoap.org/ws/2004/09/enumeration/PullResponse";
   }
}