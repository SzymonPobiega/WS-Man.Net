using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WSMan.NET.Management;
using WSMan.NET.Transfer;
using WSMan.NET.Web;

namespace ManagementWebDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public const string ResourceUri = "http://jsr262.dev.java.net/DynamicMBeanResource";

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            var handler = new ManagementTransferRequestHandler();
            handler.Bind(ResourceUri, new Handler());
            var transferServer = new TransferServer(handler);

            routes.Add(new Route("management", new SOAPRouteHandler(transferServer)));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}