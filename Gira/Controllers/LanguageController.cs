/*using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Gira.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult SetLanguage(string language)
        {       
            Thread.CurrentThread.CurrentCulture = 
                CultureInfo.CreateSpecificCulture(language);

            Thread.CurrentThread.CurrentUICulture = 
                new CultureInfo(language.ToLower());

            HttpCookie cultureCookie = new HttpCookie("LanguageCookie");
            cultureCookie.Value = language;
            Response.Cookies.Add(cultureCookie);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}*/