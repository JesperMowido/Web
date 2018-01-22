using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BC.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            if (!Request.IsLocal)
            {
                if (HttpContext.Current.Request.Url.Host.Contains("mowido.se"))
                {
                    if ((!HttpContext.Current.Request.Url.Scheme.ToLower().Equals("https") ||
                        !HttpContext.Current.Request.Url.Host.StartsWith("www.mowido.se")) && !HttpContext.Current.Request.Url.IsLoopback)
                    {
                        UriBuilder builder = new UriBuilder(HttpContext.Current.Request.Url);

                        builder.Scheme = "https";
                        builder.Host = "www.mowido.se";
                        builder.Port = -1;

                        Response.StatusCode = 301;
                        Response.AddHeader("Location", builder.Uri.ToString());
                        Response.End();
                    }
                }
            }   
        }
    }
}
