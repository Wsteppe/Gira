using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Gira.Controllers
{
    [AllowAnonymous]
    public class LanguageController : Controller
    {
        public ActionResult SetLanguage(string language)
        {
            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture =
                new CultureInfo(language.ToLower());

            var cultureCookie = new HttpCookie("LanguageCookie") {Value = language};
            Response.Cookies.Add(cultureCookie);

            return Redirect(Request.UrlReferrer?.ToString());
        }
    }
}