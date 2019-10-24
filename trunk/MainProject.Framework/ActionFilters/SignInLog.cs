using System;
using System.Linq;
using System.Web.Mvc;
using MainProject.Framework.Helper;

namespace MainProject.Framework.ActionFilters
{
    public class SignInLog : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var db = DalHelper.InvokeDbContext();
            if (db != null)
            {
                var currentIP = filterContext.HttpContext.Request.UserHostAddress;
                var currentUserName = filterContext.HttpContext.User.Identity.Name;

                var userProfile =
                    db.UserProfiles.FirstOrDefault(c => c.UserName.Equals(currentUserName, StringComparison.OrdinalIgnoreCase));
                if (userProfile != null)
                {
                    userProfile.LastedIP = currentIP;
                }
                db.SaveChanges();
            }
        }
    }
}
