using System;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Gira.Data;

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
            UnityConfig.RegisterComponents();
            Database.SetInitializer(new GiraInitializer());
        }
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            var cultureCookie = Request.Cookies["LanguageCookie"];
            if (cultureCookie?.Value != null)
            {
                Thread.CurrentThread.CurrentCulture =
                                    new System.Globalization.CultureInfo(cultureCookie.Value);
                Thread.CurrentThread.CurrentUICulture =
                                    new System.Globalization.CultureInfo(cultureCookie.Value);
            }
            else
            {
                if (Request.UserLanguages == null || Request.UserLanguages.Length <= 0) return;
                var culture = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(culture);
            }

        }

    }
}
