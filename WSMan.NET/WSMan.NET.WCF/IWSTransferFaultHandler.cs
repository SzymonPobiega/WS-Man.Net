using System;
using System.ServiceModel.Channels;

namespace WSMan.NET.WCF
{
   public interface IWSTransferFaultHandler
   {
      Exception HandleFault(Message faultMessage);
   }
}