using System;
using WSMan.NET.Server;
using WSMan.NET.Transfer;

namespace TransferDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverUrl = "http://localhost:12345/";

            var transferServer = new TransferServer(new RequestHandler());
            using (new HttpListenerTransferEndpoint(serverUrl, transferServer))
            {
                var transferClient = new SOAPClient(serverUrl);

                Console.Write("Create...");
                var address = transferClient.BuildMessage().Create(new SampleData());
                Console.WriteLine(address.Address);
                Console.Write("Put...");
                var data = transferClient.BuildMessage().Put<SampleData>(new SampleData {A = "AAA"});
                Console.WriteLine(data.A);
                Console.Write("Get...");
                data = transferClient.BuildMessage().Get<SampleData>();
                Console.WriteLine(data.A);
                Console.Write("Delete...");
                transferClient.BuildMessage();
                Console.WriteLine("OK");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }

}
