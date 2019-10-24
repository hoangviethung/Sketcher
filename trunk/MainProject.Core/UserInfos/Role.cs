using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainProject.Core.UserInfos
{
    [Table("webpages_Roles")]
    public class Role
    {
        public Role()
        {
            RolePrivilleges = new List<RolePrivillege>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool IsSystem { get; set; }

        public virtual List<RolePrivillege> RolePrivilleges { get; set; }
    }
}
