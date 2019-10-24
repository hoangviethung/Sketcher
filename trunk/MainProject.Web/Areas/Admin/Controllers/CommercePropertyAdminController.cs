using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Web.BaseControllers;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Service.CommerceProperty;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.CommerceProperty;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class CommercePropertyAdminController : BaseController
    {
        private readonly CommercePropertyService _commercePropertyService;

        public CommercePropertyAdminController()
        {
            _commercePropertyService = new CommercePropertyService(DbContext);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProperty, Permission = PermissionCollection.View)]
        public ActionResult Index(int page = 1)
        {
            TempData["CurrentMenu"] = "CommerceProperty";
            return View(_commercePropertyService.GetIndex(page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProperty, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            TempData["CurrentMenu"] = "CommerceProperty";
            return View("Manage", _commercePropertyService.Create());
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProperty, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "CommerceProperty";
            var result = _commercePropertyService.Edit(id);
            if (result.Code == HttpStatusCodeCollection.NotFound)
            {
                TempData["message"] = result.Message;
                return Redirect("Index");
            }
            return View("Manage", result.Result);
        
        }

        [HttpPost]
        public ActionResult Manage(PropertyManageModel model)
        {
            TempData["CurrentMenu"] = "CommerceProperty";
            if (ModelState.IsValid)
            {
                var result = _commercePropertyService.Manage(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            // Rebind Select List
            _commercePropertyService.ReInitManageModel(model);

            return View("Manage", model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProperty, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(long id)
        {
            TempData["CurrentMenu"] = "CommerceProperty";
            TempData["message"] = _commercePropertyService.Delete(id);
            return RedirectToAction("Index", new { });
        }

    }
}
