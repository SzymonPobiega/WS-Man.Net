using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using log4net;
using WSMan.NET.Addressing;
using WSMan.NET.Server;
using WSMan.NET.SOAP;

namespace WSMan.NET.Web
{
    public class SOAPHttpHandler : IHttpHandler
    {
        private const string MessageIdProperty = "MessageID";
        private const string RequestId = "RequestID";
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpListenerTransferEndpoint));
        private readonly IRequestHandler[] _handlers;

        public SOAPHttpHandler(IRequestHandler[] handlers)
        {
            _handlers = handlers;
        }

        public void ProcessRequest(HttpContext context)
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
                    responseMessage = HandleRequest(context);
                    Log.Info("Request handled.");
                }
                catch (FaultException ex)
                {
                    Log.InfoFormat("Caught {0}. Converting to fault message.", ex.GetType().Name);
                    responseMessage = ex.CreateMessage();
                }
                WriteResponse(context, responseMessage);
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected exception during processing", ex);
                context.Response.StatusCode = 500;
                context.Response.Close();
            }
        }

        private OutgoingMessage HandleRequest(HttpContext ctx)
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

        private static void WriteResponse(HttpContext ctx, OutgoingMessage outgoingMessage)
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
                ctx.Response.OutputStream.Write(buffer, 0, (int)memoryStream.Length);
            }
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

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
