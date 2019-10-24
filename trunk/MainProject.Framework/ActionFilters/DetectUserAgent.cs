using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MainProject.Framework.ActionFilters
{
    public class DetectUserAgent : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Session["UserAgent"] = filterContext.RequestContext.HttpContext.Request.UserAgent;
        }


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}
