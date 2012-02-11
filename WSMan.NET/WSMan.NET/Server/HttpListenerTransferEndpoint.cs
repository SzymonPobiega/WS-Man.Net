using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public class HttpListenerTransferEndpoint : IDisposable
    {
        private readonly IRequestHandler[] _handlers;
        private readonly HttpListener _listener;
        private bool _disposed;

        public HttpListenerTransferEndpoint(string uriPrefix, params IRequestHandler[] handlers)
        {
            _handlers = handlers;
            _listener = new HttpListener();
            _listener.Prefixes.Add(uriPrefix);
            Task.Factory.StartNew(AcceptingLoop);
        }

        private void AcceptingLoop()
        {
            _listener.Start();
            while (!_disposed && _listener.IsListening)
            {
                try
                {
                    var ctx = _listener.GetContext();
                    Task.Factory.StartNew(() => TryHandleRequest(ctx));
                }
                catch (HttpListenerException)
                {
                    //Listener shut down - swallow
                }
            }
        }

        private void TryHandleRequest(HttpListenerContext ctx)
        {
            try
            {
                OutgoingMessage responseMessage;
                try
                {
                    responseMessage = HandleRequest(ctx);
                }
                catch (FaultException ex)
                {
                    responseMessage = ex.CreateMessage();                    
                }
                WriteResponse(ctx, responseMessage);
            }
            catch (Exception ex)
            {
                ctx.Response.StatusCode = 500;
                ctx.Response.Close();
                Console.WriteLine(ex);
            }
        }

        private OutgoingMessage HandleRequest(HttpListenerContext ctx)
        {
            ctx.Response.ContentType = @"application/soap+xml; charset=utf-8";

            var reader = XmlReader.Create(ctx.Request.InputStream);
            using (var incomingMessage = new IncomingMessage(reader))
            {
                return InvokeHandlers(incomingMessage);
            }
        }

        private static void WriteResponse(HttpListenerContext ctx, OutgoingMessage outgoingMessage)
        {
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
                var buffer = memoryStream.GetBuffer();
                ctx.Response.ContentLength64 = memoryStream.Length;
                ctx.Response.OutputStream.Write(buffer, 0, (int)memoryStream.Length);
            }
            ctx.Response.Close();
        }

        private OutgoingMessage InvokeHandlers(IncomingMessage incomingMessage)
        {
            return _handlers
                .Select(x => x.Handle(incomingMessage))
                .First(x => x != null);
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