using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainProject.Core.UserInfos
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        {
            UserInRoles = new Collection<UserInRole>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public string LastedIP { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; } 

        public string GeneratePassword { get; set; }

        public bool Gender { get; set; }

    }
}
