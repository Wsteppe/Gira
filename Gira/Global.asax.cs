using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Gira.Controllers;
using Gira.Utilities;

namespace Gira
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(
                new DefaultControllerFactory(new MultiLanguageControllerActivator()));
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{language} /{controller}/{action}/{id}",
                constraints: new {language = @"(\w{2})|(\w{2}-\w{2})"},
                defaults: new
                {
                    Controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpCookie cultureCookie = Request.Cookies["LanguageCookie"];
            if (cultureCookie != null && cultureCookie.Value != null)
            {
                Thread.CurrentThread.CurrentCulture = 
                    new System.Globalization.CultureInfo(cultureCookie.Value);
                Thread.CurrentThread.CurrentUICulture = 
                    new System.Globalization.CultureInfo(cultureCookie.Value);
            }
            else
            {
                string culture = null;
                if (Context.Request.UserLanguages != null && Request.UserLanguages.Length > 0)
                {
                    culture = Request.UserLanguages[0];
                    Thread.CurrentThread.CurrentCulture = 
                        new System.Globalization.CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = 
                        new System.Globalization.CultureInfo(culture);
                }
            }
        }
        
    }
}
