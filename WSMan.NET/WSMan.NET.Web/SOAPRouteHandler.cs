using System.Web;
using System.Web.Routing;
using WSMan.NET.Server;

namespace WSMan.NET.Web
{
    public class SOAPRouteHandler : IRouteHandler
    {
        private readonly IRequestHandler[] _handlers;

        public SOAPRouteHandler(params IRequestHandler[] handlers)
        {
            _handlers = handlers;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SOAPHttpHandler(_handlers);
        }
    }
}