using System.Web;
using System.Web.Mvc;
using Gira.Utilities;

namespace Gira
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new LocalizationAttribute("en"), 0);
        }
    }
}
