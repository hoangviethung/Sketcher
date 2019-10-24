using System.Web.Mvc;
using MainProject.Framework.Helper;
using System.Linq;
using System.Web;

namespace MainProject.Framework.ActionFilters
{
    public class FilterLogonPermission : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                var dbContext = DalHelper.InvokeDbContext();
                var isValid = dbContext.UserProfiles.Any(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name) && x.IsActive);

                if (!isValid)
                {
                    var urlHelper = new UrlHelper(filterContext.RequestContext);
                    filterContext.Result = new RedirectResult(urlHelper.Action("Logon", "HomeAdmin", new { area = "Admin" }));
                }
            }
        }
    }
}
