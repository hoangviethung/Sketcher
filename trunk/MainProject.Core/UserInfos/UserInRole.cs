using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainProject.Core.UserInfos
{
    [Table("webpages_UsersInRoles")]
    public class UserInRole
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual Role Role { get; set; }
    }
}
