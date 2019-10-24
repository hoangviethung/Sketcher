using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Service.LoginHistory;
using MainProject.Web.BaseControllers;
using System.Web.Mvc;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class LoginHistoryAdminController : BaseController
    {
        private readonly LoginHistoryService _loginHistoryService;

        public LoginHistoryAdminController()
        {
            _loginHistoryService = new LoginHistoryService(DbContext);
        }

        // GET: Admin/LoginHistory
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageLoginHistory, Permission = PermissionCollection.View)]
        public ActionResult Index(string name = null, int page = 1)
        {
            //set current active menu of admin page
            TempData["CurrentMenu"] = "LoginStory";
            return View(_loginHistoryService.GetIndex(name, page));
        }

        // GET: Admin/LoginHistory
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageLoginHistory, Permission = PermissionCollection.Create)]
        [HttpPost]
        public ActionResult Download(string name)
        {
            var fileName = name + ".txt";
            if (FolderAndFileHelper.FileExist("/Upload/LoginHistory/" + fileName))
            {
                return File(FolderAndFileHelper.ReadAllBytes("/Upload/LoginHistory/" + fileName),
                            System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

            return RedirectToAction("Index");
        }
    }
}