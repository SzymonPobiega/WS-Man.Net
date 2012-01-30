using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WSMan.NET.SOAP;

namespace WSMan.NET.Transfer
{
    public class HttpListenerTransferEndpoint : IDisposable
    {
        private readonly HttpListener _listener;
        private readonly TransferServer _transferServer;
        private bool _disposed;

        public HttpListenerTransferEndpoint(string uriPrefix, TransferServer transferServer)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(uriPrefix);
            Task.Factory.StartNew(AcceptingLoop);
            _transferServer = transferServer;
        }

        private void AcceptingLoop()
        {
            _listener.Start();
            while (!_disposed && _listener.IsListening)
            {
                try
                {
                    var ctx = _listener.GetContext();
                    Task.Factory.StartNew(() => HandleRequest(ctx));
                }
                catch (HttpListenerException)
                {
                    //Listener shut down - swallow
                }
            }
        }

        private void HandleRequest(HttpListenerContext ctx)
        {
            ctx.Response.ContentType = @"application/soap+xml; charset=utf-8";

            var reader = XmlReader.Create(ctx.Request.InputStream);
            using (var incomingMessage = new IncomingMessage(reader))
            {
                var outgoingMessage = _transferServer.Handle(incomingMessage);
                byte[] buffer;
                using (var memoryStream = new MemoryStream())
                {
                    var settings = new XmlWriterSettings
                                       {
                                           Encoding = Encoding.UTF8
                                       };
                    using (var writer = XmlWriter.Create(memoryStream, settings))
                    {
                        outgoingMessage.Write(writer);
                        writer.Flush();
                    }
                    buffer = memoryStream.GetBuffer();
                }
                ctx.Response.ContentLength64 = buffer.Length;
                ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
                ctx.Response.Close();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _listener.Stop();
            _disposed = true;
        }
    }
}