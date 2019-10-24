using System.Web.Mvc;
using System.Web.Routing;

namespace MainProject.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.css/{*pathInfo}");
            routes.IgnoreRoute("{resource}.js/{*pathInfo}");
            routes.IgnoreRoute("{folder}/{*pathInfo}", new { folder = "upload" });
			
            routes.MapRoute(
                name: "Account-Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Account-Login-vi",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account-Reset-Pass",
                url: "reset-password",
                defaults: new { controller = "Account", action = "ResetPass", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Account-Reset-Pass-vi",
                url: "lay-lai-mat-khau",
                defaults: new { controller = "Account", action = "ResetPass", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account-Logoff-vi",
                url: "dang-xuat",
                defaults: new { controller = "Account", action = "Logoff", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Account-Logoff",
                url: "logoff",
                defaults: new { controller = "Account", action = "Logoff", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Account-Register",
                url: "register",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Account-Register-vi",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetInfo",
                url: "GetInfo/{provider}/{username}/{userId}/{Email}",
                defaults: new { controller = "Account", action = "GetInfo", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Manage",
                url: "Manage",
                defaults: new { controller = "Account", action = "Manage", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChangePassword",
                url: "ChangePassword",
                defaults: new { controller = "Account", action = "ChangePassword", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "WebSetting",
                url: "_home-route-settings/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SiteMap",
                url: "sitemap.xml",
                defaults: new { controller = "Home", action = "sitemap" },
                namespaces: new string[] { "MainProject.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Handler",
                url: "{*url}",
                defaults: new { controller = "Common", action = "Redirect" }
            );

            //routes.MapRoute(
            //    "CustomRoute",
            //    "{url}",
            //    new { controller = "Common", action = "Redirect" }
            //    //new { url = @"^([a-z0-9]-?)+$" }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}