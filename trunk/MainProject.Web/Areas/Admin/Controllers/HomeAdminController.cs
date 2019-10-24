using System.Web.Mvc;
using System.Web.Security;
using MainProject.Web.BaseControllers;
using MainProject.Framework.ActionFilters;
using MainProject.Core.Enums;
using MainProject.ServiceAdmin.Service.Home;
using MainProject.ServiceAdmin.Service.Account;
using MainProject.ServiceAdmin.Model.User;

namespace MainProject.Web.Areas.Admin.Controllers
{
    [FilterLogonPermission]
    public class HomeAdminController : BaseController
    {
        private readonly HomeService _service;
        private readonly AccountService _accountService;
        public HomeAdminController()
        {
            _service = new HomeService(DbContext);
            _accountService = new AccountService(DbContext);
        }
        
        public ActionResult Index()
        {
            //if (!(new CurrentUserHelper(Request).CheckHasPermissionOnFeature(
            //        EntityManageTypeCollection.ManageStatistical, PermissionCollection.View)))
            //            return View();

            return View(_service.GetIndex());
        }

        [AllowAnonymous]
        public ActionResult ForgetPass()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgetPass(ForgetPass model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.UpdateForgetPass(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Logon", "HomeAdmin");
                }
                ModelState.AddModelError("", "Tải khoản chưa được cho phép hoạt động.");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logon()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Logon(UserLogon model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.Login(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", result.Message);
            }

            return View(model);
        }

        //[AllowAnonymous]
        //public string ResetPasswordForDefaultAccount(string key = "")
        //{
        //    if (string.IsNullOrEmpty(key))
        //    {
        //        var str = DateTime.Now.ToFileTimeUtc().ToString();
        //        var strKey = "*" + str.Substring(str.Length - 4) + "!";
        //        Session["resetKey"] = strKey;

        //        return str;
        //    }
        //    var value = Session["resetKey"];
        //    if (value != null)
        //    {
        //        if (key.Equals((string)value))
        //        {
        //            var user = Membership.GetUser(StringConstant.DefaultAdministrator);
        //            if (user == null)
        //            {
        //                WebSecurity.CreateUserAndAccount(StringConstant.DefaultAdministrator, "@123456",
        //                                                 new {Email = "admin@web4g.com", IsActive = true});
        //                Roles.AddUserToRole(StringConstant.DefaultAdministrator, RoleName.Admin);

        //                return "created!";
        //            }
        //            else
        //            {
        //                if (!Roles.IsUserInRole(StringConstant.DefaultAdministrator, RoleName.Admin))
        //                {
        //                    Roles.AddUserToRole(StringConstant.DefaultAdministrator, RoleName.Admin);
        //                    return "assigned!";
        //                }
        //                else
        //                {
        //                    var token = WebSecurity.GeneratePasswordResetToken(StringConstant.DefaultAdministrator);
        //                    WebSecurity.ResetPassword(token, "@123456");
        //                    return "finished!";
        //                }
        //            }
        //        }
        //    }
        //    return "nothing";
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Logon", "HomeAdmin", new { area = "Admin" });
        }

        public ActionResult Ckfinder()
        {
            return View();
        }

        public ActionResult CkfinderPopup()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult BackupDatabase()
        //{
        //    return Json(_service.BackupDB(), JsonRequestBehavior.AllowGet);
        //}
    }
}
