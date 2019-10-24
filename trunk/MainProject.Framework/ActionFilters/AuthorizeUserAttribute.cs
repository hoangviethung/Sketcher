using System.Web;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;

namespace MainProject.Framework.ActionFilters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public PermissionCollection Permission { get; set; }

        public EntityManageTypeCollection Feature { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            var dbContext = DalHelper.InvokeDbContext();
            var roleRepository = new RoleRepository(dbContext);

            return roleRepository.VerifyUserPermissionOfAction(httpContext.User.Identity.Name, Permission, Feature);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(urlHelper.Action("Logon", "HomeAdmin", new { area = "Admin" }));
        }
    }
}
