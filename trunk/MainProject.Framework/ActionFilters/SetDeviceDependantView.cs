using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MainProject.Framework.ActionFilters
{
    public class SetDeviceDependantView : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Only works on ViewResults...
            if (filterContext.Result is ViewResultBase)
            {
                var viewResult = filterContext.Result as ViewResultBase;
                // Default the viewname to the action name
                if (String.IsNullOrEmpty(viewResult.ViewName))
                    viewResult.ViewName = filterContext.RouteData.GetRequiredString("action");

                // Add suffix according to device type
                if (IsMobile(filterContext.HttpContext))
                    viewResult.ViewName += "Mobile";
            }
            base.OnResultExecuting(filterContext);
        }

        private static bool IsMobile(HttpContextBase httpContext)
        {
            return httpContext.Request.Browser.IsMobileDevice;
        }

        private static bool IsTablet(HttpContextBase httpContext)
        {
            // this requires the 51degrees "Device Data" package: http://51degrees.mobi/Products/DeviceData/PropertyDictionary.aspx
            var isTablet = httpContext.Request.Browser["IsTablet"];
            return isTablet != null && isTablet.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
