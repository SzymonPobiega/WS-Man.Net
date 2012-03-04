using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using WSMan.NET.Addressing;
using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public class HttpListenerTransferEndpoint : IDisposable
    {
        private const string MessageIdProperty = "MessageID";
        private const string RequestId = "RequestID";
        private static readonly ILog Log = LogManager.GetLogger(typeof (HttpListenerTransferEndpoint));

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
            Log.Info("Starting accepting loop.");
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
            ThreadContext.Properties.Clear();
            var uniqueRequestId = Guid.NewGuid().ToString("N");
            ThreadContext.Properties[RequestId] = uniqueRequestId;
            Log.Info("Starting handling request.");                    
            try
            {
                OutgoingMessage responseMessage;
                try
                {
                    responseMessage = HandleRequest(ctx);
                    Log.Info("Request handled.");
                }
                catch (FaultException ex)
                {
                    Log.InfoFormat("Caught {0}. Converting to fault message.", ex.GetType().Name);
                    responseMessage = ex.CreateMessage();                    
                }
                WriteResponse(ctx, responseMessage);
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected exception during processing", ex);
                ctx.Response.StatusCode = 500;
                ctx.Response.Close();
            }
        }

        private OutgoingMessage HandleRequest(HttpListenerContext ctx)
        {
            ctx.Response.ContentType = @"application/soap+xml; charset=utf-8";            
            var reader = XmlReader.Create(ctx.Request.InputStream);
            using (var incomingMessage = new IncomingMessage(reader))
            {
                var messageIdHeader = incomingMessage.GetHeader<MessageIdHeader>();
                if (messageIdHeader != null)
                {
                    ThreadContext.Properties[MessageIdProperty] = messageIdHeader.MessageId;
                }
                Log.Debug("Invoking request handlers.");
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
                    Log.Debug("Writing response to stream.");
                    outgoingMessage.Write(writer);
                    writer.Flush();
                }
                var buffer = memoryStream.GetBuffer();
                ctx.Response.ContentLength64 = memoryStream.Length;
                ctx.Response.OutputStream.Write(buffer, 0, (int)memoryStream.Length);
            }
            ctx.Response.Close();
        }

        private OutgoingMessage InvokeHandlers(IncomingMessage message)
        {
            return _handlers
                .Select(x => InvokeHandler(x, message))
                .First(x => x != null);
        }

        private static OutgoingMessage InvokeHandler(IRequestHandler handler, IncomingMessage message)
        {
            Log.DebugFormat("Invoking {0}.", handler.GetType().Name);
            var result = handler.Handle(message);
            Log.Debug(result != null ? "Handled." : "Ignored.");
            return result;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            Log.Info("Closing.");
            _listener.Stop();
            _disposed = true;
        }
    }
}