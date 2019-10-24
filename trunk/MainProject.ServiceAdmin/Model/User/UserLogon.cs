using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model.User
{
    public class UserLogon
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgetPass
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email nhận mật khẩu!")]
        public string Email { get; set; }
    }
}