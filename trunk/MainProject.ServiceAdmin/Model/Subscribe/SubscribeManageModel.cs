using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model.Subscribe
{
    public class SubscribeManageModel
    {
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string Note { get; set; }
    }
}
