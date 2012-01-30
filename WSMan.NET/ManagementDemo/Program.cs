using System;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace ManagementDemo
{
    class Program
    {
        public const string ResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";

        static void Main(string[] args)
        {
            var handler = new ManagementTransferRequestHandler();
            handler.Bind(new Uri(ResourceUri), new Handler());

            var transferServer = new TransferServer(handler);
            using (new HttpListenerTransferEndpoint("http://localhost:12345/", transferServer))
            {
                Client.PerformTest();
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}