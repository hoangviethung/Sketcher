using System.Collections.Generic;
using System.Linq;
using MainProject.Core.Enums;
using MainProject.Core.UserInfos;

namespace MainProject.Data.Repositories
{
    public class RoleRepository : AbstractMainProjectRepository<Role>
    {
        const string AdminRoleName = "Admin";
        private MainDbContext _dbContext;

        public RoleRepository(MainDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public Role CreateRole(string roleName, string description)
        {
            var role = _dbContext.Roles.FirstOrDefault(c => c.RoleName == roleName);
            if (role == null)
            {
                role = new Role
                {
                    IsSystem = false,
                    RoleDescription = description,
                    RoleName = roleName
                };
                _dbContext.Roles.Add(role);
                _dbContext.SaveChanges();
            }

            return role;
        }

        public void RemoveRolePrivillege(int rolePrivillegeId)
        {
            var item = _dbContext.RolePrivilleges.FirstOrDefault(x => x.Id == rolePrivillegeId);
            if (item != null)
            {
                _dbContext.RolePrivilleges.Remove(item);
                _dbContext.SaveChanges();
            }
        }

        public string RemoveRole(int roleId)
        {
            var item = _dbContext.Roles.FirstOrDefault(x => x.RoleId == roleId && !x.RoleName.Equals("Admin") && !x.RoleName.Equals("Guest"));
            if (item != null)
            {
                foreach (var rolePrivillege in item.RolePrivilleges.ToList())
                {
                    _dbContext.RolePrivilleges.Remove(rolePrivillege);
                }
                _dbContext.Roles.Remove(item);
                _dbContext.SaveChanges();

                return "<strong style='color:red'>Xóa nhóm quyền thành công!</strong>";
            }

            return "<strong style='color:red'>Không thể xóa nhóm quyền này! Đã có lỗi xảy ra!</strong>";
        }

        public List<RolePrivillege> GetRolePrivillegesOfUser(string username)
        {
            var result = new List<RolePrivillege>();
            var user = _dbContext.UserProfiles.FirstOrDefault(c => c.UserName.Equals(username));
            if (user != null)
            {
                result = user.UserInRoles.SelectMany(x => x.Role.RolePrivilleges.Select(rp => rp)).ToList();
            }

            return result;
        }

        public bool VerifyUserPermissionOfAction(string username, PermissionCollection permission,
            EntityManageTypeCollection feature)
        {
            var user = _dbContext.UserProfiles.FirstOrDefault(c => c.UserName.Equals(username));
            if (user != null)
            {
                return user.UserInRoles.Any(x => x.Role.RoleName.Equals(AdminRoleName)
                                                 || x.Role.RolePrivilleges.Any(
                                                     rp => rp.PermissionType == permission &&
                                                           rp.FeatureType == feature));
            }
            return false;
        }

        public bool VerifyUserPermissionOfFeature(string username, EntityManageTypeCollection feature)
        {
            var user = _dbContext.UserProfiles.FirstOrDefault(c => c.UserName.Equals(username));
            if (user != null)
            {
                return user.UserInRoles.Any(x => x.Role.RoleName.Equals(AdminRoleName) ||
                                                 x.Role.RolePrivilleges.Any(rp => rp.FeatureType == feature));
            }
            return false;
        }
    }
}
