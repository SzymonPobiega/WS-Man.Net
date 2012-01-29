using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel.Channels;

namespace WSMan.NET.Transfer
{
   public interface IWSTransferFaultHandler
   {
      Exception HandleFault(Message faultMessage);
   }
}