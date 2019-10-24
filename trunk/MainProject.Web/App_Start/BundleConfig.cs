using System.Web.Optimization;

namespace MainProject.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
            ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                 "~/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
               "~/Scripts/jquery.validate.min.js",
               // Ajax.BeginForm
               "~/Scripts/jquery.unobtrusive-ajax.min.js",
               // Html.BeginForm
               "~/Scripts/jquery.validate.unobtrusive.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                // Switch language
                "~/Scripts/MainProject/CommonController.js"
            ));

            // Code removed for clarity.
            BundleTable.EnableOptimizations = true;
        }
    }
}