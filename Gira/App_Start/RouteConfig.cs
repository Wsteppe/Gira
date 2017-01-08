using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gira
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*
            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{language}/{controller}/{action}/{id}",
                constraints: new {language = @"(\w{2})|(\w{2}-\w{2})"},
                defaults: new
                {
                    Controller = "Home",
                    Action = "Index",
                    id = UrlParameter.Optional
                }
            );
            */

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional }
            );
        }
    }
}
