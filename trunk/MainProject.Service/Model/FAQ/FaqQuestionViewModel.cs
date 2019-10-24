using System.ComponentModel.DataAnnotations;
using MainProject.Core;

namespace MainProject.Service.Model.FAQ
{
    public class FaqQuestionViewModel
    {
        [Required(ErrorMessage = "Chua_nhap_ho_ten")]
        public string AskerName { get; set; }

        [StringLength(20, ErrorMessage = "So_dien_thoai_khong_hop_le")]
        [RegularExpression("([0-9]+)", ErrorMessage = "So_dien_thoai_khong_hop_le")]
        [Required(ErrorMessage = "Chua_nhap_so_dien_thoai")]
        public string AskerPhone { get; set; }

        [Required(ErrorMessage = "Chua_nhap_dia_chi_email")]
        [EmailAddress(ErrorMessage = "Dia_chi_email_khong_hop_le")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Dia_chi_email_khong_hop_le")]
        public string AskerEmail { get; set; }

        public string AskerAddress { get; set; }

        public string AskTitle { get; set; }

        [Required(ErrorMessage = "Chua_nhap_noi_dung")]
        public string AskContent { get; set; }

        public string Message { get; set; }

        public bool Result { get; set; }

        public string Breadcrumb { get; set; }

        public FaqQuestionViewModel() { }

        public FaqQuestionViewModel(FaqItem entity)
        {
            AskerName = entity.AskerName;
            AskerPhone = entity.AskerPhone;
            AskerEmail = entity.AskerEmail;
            AskerAddress = entity.AskerAddress;
            AskTitle = entity.AskTitle;
            AskContent = entity.AskContent;
        }

        public static void ToEntity(FaqQuestionViewModel model, ref FaqItem entity)
        {
            entity.AskerName = model.AskerName;
            entity.AskerPhone = model.AskerPhone;
            entity.AskerEmail = model.AskerEmail;
            entity.AskerAddress = model.AskerAddress;
            entity.AskTitle = model.AskTitle;
            entity.AskContent = model.AskContent;
        }
    }
}