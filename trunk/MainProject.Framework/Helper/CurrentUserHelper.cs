using System.Web;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;

namespace MainProject.Framework.Helper
{
    public class CurrentUserHelper
    {
        private RoleRepository _roleRepository;
        private HttpRequestBase _request;

        public CurrentUserHelper(HttpRequestBase request)
        {
            var db = DalHelper.InvokeDbContext();
            _roleRepository = new RoleRepository(db);
            _request = request;
        }

        public bool CheckHasPermissionOnFeature(EntityManageTypeCollection feature)
        {
            if (!_request.IsAuthenticated) return false;

            return _roleRepository.VerifyUserPermissionOfFeature(_request.RequestContext.HttpContext.User.Identity.Name,
                feature);
        }

        public bool CheckHasPermissionOnFeature(EntityManageTypeCollection feature, PermissionCollection permission)
        {
            if (!_request.IsAuthenticated) return false;

            return _roleRepository.VerifyUserPermissionOfAction(_request.RequestContext.HttpContext.User.Identity.Name, permission,
                feature);
        }

        public bool IsCurrentUser(string userName)
            => _request.RequestContext.HttpContext.User.Identity.Name.ToLower().Equals(userName.ToLower());
    }
}
