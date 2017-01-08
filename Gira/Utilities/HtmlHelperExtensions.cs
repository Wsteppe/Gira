/*using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gira.Utilities
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ActionLink(
            this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            string htmlAttributes,
            CultureInfo cultureInfo)
        {
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;

            string localizedControllerName = String.Format("{0}/{1}",
                cultureInfo.TwoLetterISOLanguageName, controllerName);

            return helper.ActionLink(linkText, actionName, localizedControllerName,
                routeValues, htmlAttributes, cultureInfo);
        }
    }
}
*/