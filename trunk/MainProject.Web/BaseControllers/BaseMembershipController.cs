using System.Web.Mvc;
using System.Web.Security;
using MainProject.Data;
using MainProject.Framework.Helper;
using WebMatrix.WebData;

namespace MainProject.Web.BaseControllers
{
    public class BaseMembershipController : BaseController
    {
        public SimpleRoleProvider RoleProvider
        {
            get
            {
                return (SimpleRoleProvider)Roles.Provider;
            }
        }

        public SimpleMembershipProvider MembershipProvider
        {
            get
            {
                return (SimpleMembershipProvider)Membership.Provider;
            }
        }
    }
}
