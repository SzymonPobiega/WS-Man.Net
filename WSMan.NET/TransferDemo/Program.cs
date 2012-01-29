using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using WSMan.NET.Transfer;
using WSMan.NET.WCF;
using MessageVersion = System.ServiceModel.Channels.MessageVersion;

namespace TransferDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var transferServer = new TransferServer(new RequestHandler());
            using (var transferEndpoint = new HttpListenerTransferEndpoint("http://localhost:12345/", transferServer))
            {
                var channelFactory = new ChannelFactory<IWSTransferContract>(new BasicHttpBindingWithAddressing());
                var transferClient = new TransferClient(new Uri("http://localhost:12345/Transfer"),
                                                        channelFactory,
                                                        MessageVersion.Soap12WSAddressingAugust2004,
                                                        new WSTransferFaultHandler());

                Console.Write("Create...");
                var address = transferClient.Create(x => { }, x => { }, new SampleData());
                Console.WriteLine(address.Uri);
                Console.Write("Put...");
                var data = transferClient.Put<SampleData>(x => { }, x => { }, new SampleData {A = "AAA"});
                Console.WriteLine(data.A);
                Console.Write("Get...");
                data = transferClient.Get<SampleData>(x => { }, x => { });
                Console.WriteLine(data.A);
                Console.Write("Delete...");
                transferClient.Delete(x => { }, x => { });
                Console.WriteLine("OK");
            }
        }

        private class WSTransferFaultHandler : IWSTransferFaultHandler
        {
            public Exception HandleFault(Message faultMessage)
            {
                return new FaultException(MessageFault.CreateFault(faultMessage, int.MaxValue));
            }
        }
    }

}
