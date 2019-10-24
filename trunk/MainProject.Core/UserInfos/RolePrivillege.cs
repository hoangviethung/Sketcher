using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core.UserInfos
{
    public class RolePrivillege
    {
        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }

        public  PermissionCollection PermissionType { get; set; }

        public  EntityManageTypeCollection FeatureType { get; set; }
    }
}
