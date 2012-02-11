using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public class SOAPClient
    {
        private readonly string _serverUrl;

        public SOAPClient(string serverUrl)
        {
            _serverUrl = serverUrl;
        }

        public IMessageBuilder BuildMessage()
        {
            return new MessageBuilder(this);
        }

        public IncomingMessage SendRequest(OutgoingMessage requestMessage)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create(_serverUrl);
            httpRequest.Method = "POST";
            httpRequest.MediaType = "application/soap+xml; charset=utf-8";
            SerializeRequestBody(httpRequest, requestMessage);
            var response = (HttpWebResponse)httpRequest.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    return new IncomingMessage(XmlReader.Create(responseStream));
                }
                throw new SOAPException("Missing response body.");
            }
            var exceptionMessage = GetExceptionMessage(response.StatusCode);
            throw new SOAPException(exceptionMessage);
        }

        private static string GetExceptionMessage(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return "Malformed Request Message";
                case HttpStatusCode.MethodNotAllowed:
                    return "HTTP Method is neither POST nor GET";
                case HttpStatusCode.UnsupportedMediaType:
                    return "Unsupported message encapsulation method";
                default:
                    return "Unknown error " + statusCode;
            }
        }

        private static void SerializeRequestBody(HttpWebRequest httpRequest, OutgoingMessage requestMessage)
        {
            using (var memoryStream = new MemoryStream())
            {
                var settings = new XmlWriterSettings
                                   {
                                       Encoding = Encoding.UTF8
                                   };
                using (var writer = XmlWriter.Create(memoryStream, settings))
                {
                    requestMessage.Write(writer);
                    writer.Flush();
                }
                var buffer = memoryStream.GetBuffer();
                httpRequest.ContentLength = memoryStream.Length;
                var bodyStream = httpRequest.GetRequestStream();
                bodyStream.Write(buffer, 0, (int)memoryStream.Length);
                bodyStream.Close();
            }
        }
    }
}