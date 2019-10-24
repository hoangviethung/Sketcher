using System.Web.Mvc;
using MainProject.Web.BaseControllers;
using MainProject.Framework.ActionFilters;
using MainProject.Core.Enums;
using MainProject.ServiceAdmin.Service.Contact;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.Contact;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class ContactAdminController : BaseController
    {
        private readonly ContactService _contactService;

        public ContactAdminController()
        {
            _contactService = new ContactService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContact, Permission = PermissionCollection.View)]
        public ActionResult Index()
        {
            TempData["CurrentMenu"] = "ContactAdmin";
            return View(_contactService.GetIndex());
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContact, Permission = PermissionCollection.View)]
        public ActionResult Detail(long id)
        {
            TempData["CurrentMenu"] = "ContactAdmin";
            var model = _contactService.Detail(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContact, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(long id)
        {
            TempData["CurrentMenu"] = "ContactAdmin";
            TempData["message"] = _contactService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
