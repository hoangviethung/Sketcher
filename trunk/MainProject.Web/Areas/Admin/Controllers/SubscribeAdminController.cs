using MainProject.Web.BaseControllers;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Service.Subscribe;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.Subscribe;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class SubscribeAdminController : BaseController
    {
        private readonly SubscribeService _subscribeService;
        public SubscribeAdminController()
        {
            _subscribeService = new SubscribeService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageSubscribe, Permission = PermissionCollection.View)]
        public ActionResult Index(int page = 1)
        {
            // Set authorize behaviors on Index page
            TempData["CurrentMenu"] = "SubscribeAdmin";
            return View(_subscribeService.GetIndex(page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageSubscribe, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int Id)
        {
            TempData["CurrentMenu"] = "SubscribeAdmin";
            TempData["message"] = _subscribeService.Delete(Id);
            return RedirectToAction("Index");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageSubscribe, Permission = PermissionCollection.Edit)]
        [HttpPost]
        public JsonResult EditNote(SubscribeManageModel model)
        {
            TempData["CurrentMenu"] = "SubscribeAdmin";
            return Json(_subscribeService.EditNote(ModelState.IsValid, model), JsonRequestBehavior.AllowGet);
        }
    }
}
