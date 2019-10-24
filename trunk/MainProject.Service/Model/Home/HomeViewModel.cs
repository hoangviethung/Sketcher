using MainProject.Core;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Service.Model.Home
{
    public class HomeViewModel
    {
        public Category Category { get; set; }
    }

    public class SubscribeModel
    {
        [StringLength(200, ErrorMessage = "Dia_chi_email_khong_hop_le")]
        [Required(ErrorMessage = "Chua_nhap_dia_chi_email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Dia_chi_email_khong_hop_le")]
        public string Email { get; set; }
    }
}