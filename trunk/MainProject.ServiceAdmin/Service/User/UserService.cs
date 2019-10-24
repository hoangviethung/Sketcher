using MainProject.Core.Enums;
using MainProject.Core.UserInfos;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Constant;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.User;
using MainProject.ServiceAdmin.Service.LogHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MainProject.ServiceAdmin.Service.User
{
    public class UserService
    {
        private readonly UserInfoRepository _userInfoRepository;
        private readonly RoleRepository _roleRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public UserService(MainDbContext dbContext, string currentUser)
        {
            _userInfoRepository = new UserInfoRepository(dbContext);
            _roleRepository = new RoleRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.UserAdmin);
        }

        /// <summary>
        /// get all user for index 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IndexViewModel<UserProfile> GetIndex(string userName = "", int page = 1)
        {
            var query = _userInfoRepository.Find(u => !u.UserInRoles.Any(r => r.Role.RoleName.Equals(RoleName.Guest))).Distinct();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(u => u.UserName.Equals(userName));
            }

            return new IndexViewModel<UserProfile>()
            {
                ListItems = query.OrderBy(x => x.UserId).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                PagingViewModel = new PagingModel(query.Count(), _itemPerPage, page, "href='/Admin/UserAdmin/Index?username=" + userName + "&page={0}'")
            };
        }

        /// <summary>
        /// Initialize data for create view
        /// </summary>
        /// <returns></returns>
        public UserManageCreateViewModel Create()
        {
            return new UserManageCreateViewModel
            {
                Roles = _roleRepository.Find(x => !x.RoleName.Equals(RoleName.Guest))
                                       .Select(x => new SelectListItem() {
                                           Text = x.RoleName,
                                           Value = x.RoleName.ToString()
                                       }).ToList(),
                RoleSelectedValues = new List<string>()
            };
        }

        /// <summary>
        /// Insert data user into db
        /// </summary>
        /// <param name="userManageViewModel"></param>
        public BaseResponseModel Insert(UserManageCreateViewModel model)
        {
            try
            {
                // Check user name is exist
                if (!AccountHelper.CheckUserExist(model.UserName))
                {
                    // Check email is exist
                    if (!_userInfoRepository.CheckEmailExist(model.Email))
                    {
                        // Check format password
                        if (AccountHelper.CheckPassword(model.Password))
                        {
                            // Create user
                            AccountHelper.CreateUserAndAccount(model.UserName, model.Password, new {
                                                               model.Email, model.IsActive, Gender = true
                                                             });
                            // Get user to update log history
                            var user = _userInfoRepository.FindUnique(x => x.UserName == model.UserName);
                            // Save log history
                            _logHistoryService.Create(new LogHistoryModel { EntityId = user.UserId, ActionType = ActionTypeCollection.Create });

                            // Add user to role
                            Roles.AddUserToRoles(model.UserName, model.RoleSelectedValues.ToArray());

                            return new BaseResponseModel()
                            {
                                Code = HttpStatusCodeCollection.OK,
                                Message = string.Format("<strong style='color:green'>Tài khoản đã được tạo!</strong>")
                            };
                        }

                        return new BaseResponseModel()
                        {
                            Code = HttpStatusCodeCollection.BadRequest,
                            Message = string.Format("Mật khẩu phải chứa ít nhất 1 ký hiệu đặc biệt và 1 số, xin chọn mật khẩu khác!")
                        };
                    }

                    return new BaseResponseModel()
                    {
                        Code = HttpStatusCodeCollection.BadRequest,
                        Message = string.Format("Email đã có, xin chọn Email khác!")
                    };
                }

                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("Tên tài khoản đã có, xin chọn tên khác!")
                };
            }
            catch (Exception ex) { }

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = string.Format("Đã có lỗi xảy ra!")
            };
        }

        /// <summary>
        /// Get data for edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<UserManageEditViewModel> Edit(int id)
        {
            // Get entity to bind model
            var entity = _userInfoRepository.FindUnique(x => x.UserId == id);
            // Check entity is exist
            if (entity == null)
            {
                return new BaseResponseModel<UserManageEditViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Không tìm thấy user cần sửa</strong>")
                };
            }

            // Bind entity to model
            var model = new UserManageEditViewModel(entity);
            RebindSelectListItem(model);

            return new BaseResponseModel<UserManageEditViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel UpdateProfile(UserManageEditViewModel model)
        {
            // Get entity to update
            var user = _userInfoRepository.FindUnique(x => x.UserName.Equals(model.UserName));
            // Bind model to entity
            user.Email = model.Email;
            // Save entity to db
            _userInfoRepository.SaveChanges();
            // Get user role in db
            var roles = Roles.GetRolesForUser(model.UserName);
            if (roles != null && roles.Any())
            {
                // Delete all to add new role
                Roles.RemoveUserFromRoles(model.UserName, roles);
            }
            // Add user to role
            Roles.AddUserToRoles(model.UserName, model.RoleSelectedValues.ToArray());

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Cập nhật thông tin thành công!</strong>"),
            };
        }

        /// <summary>
        /// Get data for edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<UserManagePasswordModel> EditPassword(int id)
        {
            // Get entity to bind model
            var entity = _userInfoRepository.FindUnique(x => x.UserId == id);
            // Check entity is exist
            if (entity == null)
            {
                return new BaseResponseModel<UserManagePasswordModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Không tìm thấy user cần sửa</strong>")
                };
            }

            return new BaseResponseModel<UserManagePasswordModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = new UserManagePasswordModel() {
                    UserName = entity.UserName
                }
            };
        }

        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel UpdatePassword(UserManagePasswordModel model)
        {
            // Get entity to update
            var currentUser = AccountHelper.MemberGetUser(model.UserName);
            // Check entity is exist
            if (currentUser != null)
            {
                // Check password format
                if (AccountHelper.CheckPassword(model.NewPassword))
                {
                    // Change password
                    if (currentUser.ChangePassword(model.Password, model.NewPassword))
                    {
                        // save logHistory 
                        _logHistoryService.Create(new LogHistoryModel { EntityId = Convert.ToInt32(currentUser.ProviderUserKey), ActionType = ActionTypeCollection.Edit });

                        return new BaseResponseModel()
                        {
                            Code = HttpStatusCodeCollection.OK,
                            Message = string.Format("<strong style='color:green'>Đổi mật khẩu thành công!</strong>")
                        };
                    }

                    return new BaseResponseModel()
                    {
                        Code = HttpStatusCodeCollection.BadRequest,
                        Message = string.Format("<strong style='color:red'>Mật khẩu cũ không đúng!</strong>"),
                    };
                }

                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Mật khẩu phải chứa ít nhất 1 ký hiệu đặc biệt và 1 số, xin chọn mật khẩu khác!</strong>"),
                };
            }
            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = string.Format("<strong style='color:red'>Tài khoản không tồn tại!</strong>"),
            };
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string Delete(int id, string userName)
        {
            // Get user is deleted
            var user = _userInfoRepository.FindId(id);
            // Check user is exist
            if (user == null)
            {
                return string.Format("<strong style='color:green'>Không tìm thấy user</strong>");
            }
            // Make sure user is deleted not current user
            else if (userName.Equals(user.UserName))
            {
                return string.Format("<strong style='color:green'>Không thể xóa tài khoản khi đang đăng nhập!</strong>");
            }
            else
            {
                // Remove user's roles
                if (Roles.GetRolesForUser(user.UserName).Count() > 0)
                {
                    Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                }
                // Delete user
                AccountHelper.DeleteAccount(user.UserName);
                AccountHelper.DeleteUser(user.UserName, true);
                // Save history
                _logHistoryService.Insert(new LogHistoryModel() { EntityId = user.UserId, ActionType = ActionTypeCollection.Delete });
            }
                
            return string.Format("<strong style='color:green'>Đã xóa user thành công</strong>");
        }

        

        /// <summary>
        /// Get all role user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public BaseResponseModel GetAllRolesOfUser(string userName)
        {
            var roles = Roles.GetRolesForUser(userName);
            string rolesString = "";
            if (roles != null)
            {
                for (int i = 0; i < roles.Count(); i++)
                {
                    rolesString += roles[i].ToString() + ",";
                }
                if (!string.IsNullOrWhiteSpace(rolesString))
                {
                    return new BaseResponseModel()
                    {
                        Code = HttpStatusCodeCollection.OK,
                        Message = string.Format(rolesString.Substring(0, rolesString.Length - 1))
                    };
                }
            }
            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = string.Format("<strong style='color:red'>Đã xảy ra lỗi!</strong>")
            };
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <param name="model"></param>
        public void RebindSelectListItem(UserManageCreateViewModel model)
        {
            model.Roles = _roleRepository.Find(x => !x.RoleName.Equals(RoleName.Guest))
                                       .Select(x => new SelectListItem() {
                                           Text = x.RoleName,
                                           Value = x.RoleName
                                       }).ToList();
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <param name="model"></param>
        public void RebindSelectListItem(UserManageEditViewModel model)
        {
            model.RoleSelectedValues = Roles.GetRolesForUser(model.UserName).ToList();
            model.Roles = _roleRepository.Find(x => !x.RoleName.Equals(RoleName.Guest))
                                         .ToList()
                                         .Select(x => new SelectListItem() {
                                             Text = x.RoleName,
                                             Value = x.RoleName,
                                         }).ToList();
        }

    }
}
