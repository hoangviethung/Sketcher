using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model.User
{
    public class UserManageCreateViewModel
    {
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Độ dài tài khoản không quá 50 ký tự!")]
        [Required(ErrorMessage = "Vui lòng điền tên tài khoản!")]
        [RegularExpression("([0-9a-zA-Z]+)", ErrorMessage = "Tên tài khoản không được có ký tự đặc biệt!")]
        public string UserName { get; set; }

        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu từ 6 đến 50 ký tự!")]
        [Required(ErrorMessage = "Vui lòng điền mật khẩu!")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không chính xác.")]
        [Required(ErrorMessage = "Vui lòng điền mật khẩu!")]
        public string RePassword { get; set; }

        [StringLength(100, ErrorMessage = "Độ dài Email không quá 100 ký tự!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không hợp lệ.")]
        [Required(ErrorMessage = "Vui lòng điền Email!")]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quyền truy cập!")]
        public List<string> RoleSelectedValues { get; set; }
        public List<System.Web.Mvc.SelectListItem> Roles { get; set; }

        public UserManageCreateViewModel() { }
    }
}