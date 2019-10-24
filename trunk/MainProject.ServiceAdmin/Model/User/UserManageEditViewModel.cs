using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MainProject.ServiceAdmin.Model.User
{
    public class UserManageEditViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        [RegularExpression("([0-9a-zA-Z]+)", ErrorMessage = "Tài khoản không được dùng ký tự đặc biệt!")]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "Độ dài Email không quá 100 ký tự!")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không hợp lệ.")]
        [Required(ErrorMessage = "Vui lòng điền Email!")]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quyền truy cập!")]
        public List<string> RoleSelectedValues { get; set; }
        public List<System.Web.Mvc.SelectListItem> Roles { get; set; }

        public UserManageEditViewModel() { }

        public UserManageEditViewModel(Core.UserInfos.UserProfile entity)
        {
            Email = entity.Email;
            UserName = entity.UserName;
            IsActive = entity.IsActive;
        }
    }

    public class UserManagePasswordModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { get; set; }

        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu từ 6 đến 50 ký tự!")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới!")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu nhập lại không chính xác.")]
        public string RePassword { get; set; }

    }
}