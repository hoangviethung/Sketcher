using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model.ContactEmail
{
    public class ContactEmailManageModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsMain { get; set; }

        public ContactEmailManageModel() { }

        public ContactEmailManageModel(Core.ContactEmail entity)
        {
            Id = entity.Id;
            Email = entity.Email;
            IsActive = entity.IsActive;
            IsMain = entity.IsMain;
        }

        public static void ToEntity(ContactEmailManageModel model, ref Core.ContactEmail entity) {
            entity.Email = model.Email;
            entity.IsActive = model.IsActive;
            entity.IsMain = model.IsMain;
        }
    }
}