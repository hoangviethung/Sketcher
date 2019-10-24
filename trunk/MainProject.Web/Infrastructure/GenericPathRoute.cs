using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MainProject.Web.Infrastructure
{
    public class GenericPathRoute : Route
    {
        #region Constructor
        public GenericPathRoute(string url, IRouteHandler routeHandler) : base(url, routeHandler)
        {
        }

        public GenericPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
        {
        }

        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler) : base(url, defaults, constraints, routeHandler)
        {
        }

        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler) : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }
        #endregion

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath;
            string applicationPath = httpContext.Request.ApplicationPath;

            string rawUrl = httpContext.Request.RawUrl;
            var newVirtualPath = applicationPath;
            if (string.IsNullOrEmpty(newVirtualPath)) newVirtualPath = "/";

            newVirtualPath = applicationPath;
            newVirtualPath = "~" + newVirtualPath;
            httpContext.RewritePath(newVirtualPath, true);

            RouteData data = base.GetRouteData(httpContext);
            return data;
        }
    }
}