using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Service.Active;
using MainProject.Web.BaseControllers;
using System.Web.Mvc;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class ActiveHistoryAdminController : BaseController
    {
        private readonly ActiveService _activeService;

        public ActiveHistoryAdminController()
        {
            _activeService = new ActiveService(DbContext);
        }

        // GET: Admin/ActiveHistoty
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageLoginHistory, Permission = PermissionCollection.View)]
        public ActionResult Index(string name = null, int page = 1)
        {
            //set current active menu of admin page
            TempData["CurrentMenu"] = "ActiveStory";
            return View(_activeService.GetIndex(name, page));
        }
    }
}