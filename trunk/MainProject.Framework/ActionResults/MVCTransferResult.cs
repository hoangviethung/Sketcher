using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MainProject.Framework.ActionResults
{
    public class MVCTransferResult : RedirectResult
    {
        public MVCTransferResult(string url) : base(url)
        {
        }

        public MVCTransferResult(object routeValues)
            : base(GetRouteURL(routeValues))
        {
        }

        private static string GetRouteURL(object routeValues)
        {
            var url = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()), RouteTable.Routes);
            return url.RouteUrl(routeValues);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = HttpContext.Current;

            if (context.Controller.TempData != null && context.Controller.TempData.Any())
            {
                throw new ApplicationException("TempData won't work with Server.TransferRequest!");
            }

            httpContext.Server.TransferRequest(Url, true); // change to false to pass query string parameters if you have already processed them

            // ASP.NET MVC 2.0
            //httpContext.RewritePath(Url, false);
            //IHttpHandler httpHandler = new MvcHttpHandler();
            //httpHandler.ProcessRequest(HttpContext.Current);
        }
    }
}
