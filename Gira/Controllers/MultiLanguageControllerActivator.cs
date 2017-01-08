using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gira.Controllers
{
    public class MultiLanguageControllerActivator : IControllerActivator
    {
        private string defaultLanguage = "en";

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            string language = requestContext.RouteData.Values["language"] == null
                ? defaultLanguage
                : requestContext.RouteData.Values["language"].ToString();

            if (language != defaultLanguage)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture =
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                }
                catch (Exception e)
                {
                    throw new NotSupportedException(String.Format("ERROR: Invalid language code '{0}'.", language));
                }
            }
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }

       }
}