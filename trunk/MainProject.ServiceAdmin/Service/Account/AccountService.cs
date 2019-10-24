using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model.User;
using System;

namespace MainProject.ServiceAdmin.Service.Account
{
    public class AccountService
    {
        private readonly UserInfoRepository _userprofileRepository;
        private readonly LogHistoryRepository _logHistoryRepository;
        private readonly LanguageRepository _languageRepository;

        public AccountService(MainDbContext dbContext)
        {
            _userprofileRepository = new UserInfoRepository(dbContext);
            _logHistoryRepository = new LogHistoryRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
        }

        /// <summary>
        /// Reset an account password into db and send new random password to email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel UpdateForgetPass(ForgetPass model)
        {
            // Get account to generate password
            var userProfile = _userprofileRepository.FindUnique(d => d.Email.Equals(model.Email) && d.UserName.Equals(model.UserName));
            // Need to check this account is the one manage website and not is customer (Example: Role is Guest) but I'm so lazing to do this

            // Check account is exist
            if (userProfile == null)
            {
                return new BaseResponseModel
                {
                    Code = HttpStatusCodeCollection.NotFound,
                    Message = "Thông tin tài khoản tồn tại!"
                };
            }
            // Generate password
            string password = AccountHelper.GeneratePassword(userProfile.UserName);
            // Check password is generated
            if (!string.IsNullOrEmpty(password))
            {
                // Send password to email
                try
                {
                    MailHelper.Send(new Core.Contact { Email = userProfile.Email, Content = password }, "Lấy lại password", null);
                }
                catch(Exception ex)
                {
                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.BadGateway,
                        Message = string.Format("<span style='color:red'>Không thể gửi Email!</span>")
                    };
                }
            }

            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<span style='color:green'>Vui lòng kiểm tra Email để nhận mật khẩu!</span>")
            };
        }

        public BaseResponseModel Login(UserLogon model)
        {
            // Get user to login
            var user = _userprofileRepository.FindUnique(d => d.UserName == model.UserName);
            // Check user is exist
            if (user != null)
            {
                // Check user is active
                if (user.IsActive)
                {
                    // Log user in
                    if (AccountHelper.Login(model.UserName, model.Password, model.RememberMe))
                    {
                        // Get user IP
                        string userIP = AuthorizationHelper.GetUserIP();
                        //var LocationIP = AuthorizationHelper.GetLocationByIP(userIP);
                        // Save log history
                        var logHistory = new Core.LogHistory()
                        {
                            EntityType = EntityTypeCollection.Login,
                            ActionBy = model.UserName,
                            CreatedDate = DateTime.Now,
                            ActionType = ActionTypeCollection.Create,
                            EntityId = user.UserId,
                            Comment = "User login at " + userIP + " ("
                                        + /*(LocationIP != null ? LocationIP.country_name : "Không xác định được")  +*/
                                        ") thiết bị " + AuthorizationHelper.DetectUserAgent()
                        };
                        _logHistoryRepository.Insert(logHistory);

                        return new BaseResponseModel
                        {
                            Code = HttpStatusCodeCollection.OK
                        };
                    }
                    // If we got this far, something failed, redisplay form
                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.NotFound,
                        Message = "Tên tài khoản hoặc mật khẩu không chính xác."
                    };
                }
                return new BaseResponseModel
                {
                    Code = HttpStatusCodeCollection.NotFound,
                    Message = "Tải khoản chưa được cho phép hoạt động."
                };
            }
            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.NotFound,
                Message = "Tài khoản không tồn tại."
            };
        }
    }
}
