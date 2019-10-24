using MainProject.Framework.ActionFilters;
using System.Web;
using System.Web.Mvc;

namespace MainProject.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DetectUserAgent());
        }
    }
}