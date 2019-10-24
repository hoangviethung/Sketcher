using System;

namespace MainProject.Service.Model.Contact
{
    public class ContactManageModel
    {
        //[StringLength(200, ErrorMessage = "Chieu_dai_ho_ten_khong_duoc_qua_200_ky_tu")]
        //[Required(ErrorMessage = "Chua_nhap_ho_ten")]
        public string FullName { get; set; }

        //[RegularExpression("^[0-9]{10,11}$", ErrorMessage = "So_dien_thoai_khong_hop_le")]
        //[Required(ErrorMessage = "Chua_nhap_so_dien_thoai")]
        public string PhoneNumber { get; set; }

        //[StringLength(200, ErrorMessage = "Dia_chi_email_khong_hop_le")]
        //[Required(ErrorMessage = "Chua_nhap_dia_chi_email")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        //[StringLength(200, ErrorMessage = "Chieu_dai_khong_duoc_qua_200_ky_tu")]
        //[Required(ErrorMessage = "Chua_nhap_dia_chi")]
        public string Address { get; set; }

        //[StringLength(4000, ErrorMessage = "Chieu_dai_noi_dung_khong_duoc_qua_400_ky_tu")]
        //[Required(ErrorMessage = "Chua_nhap_noi_dung")]
        public string Content { get; set; }

        public static void ToEntity(ContactManageModel model , ref Core.Contact entity)
        {
            entity.FullName = model.FullName;
            entity.Email = model.Email;
            entity.PhoneNumber = model.PhoneNumber;
            //entity.Address = model.Address;
            entity.Content = model.Content;
        }
    }
}