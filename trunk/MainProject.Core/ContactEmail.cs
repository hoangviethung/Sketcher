using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class ContactEmail
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsMain { get; set; }
    }
}
