using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Web.BaseControllers;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Service.CommerceCategory;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.CommerceCategory;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class CommerceCategoryAdminController : BaseController
    {
        private readonly CommerceCategoryService _commerceCategoryService;
        public CommerceCategoryAdminController()
        {
            _commerceCategoryService = new CommerceCategoryService(DbContext);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceCategory, Permission = PermissionCollection.View)]
        public ActionResult Index(int page = 1)
        {
            TempData["CurrentMenu"] = "CommerceCategory";   
            return View(_commerceCategoryService.GetIndex(page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceCategory, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            TempData["CurrentMenu"] = "CommerceCategory";
            var model = _commerceCategoryService.Create();
            _commerceCategoryService.ReInitManageModel(model);
            return View("Manage", model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceCategory, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "CommerceCategory";
            var result = _commerceCategoryService.Edit(id);
            if(result.Code == HttpStatusCodeCollection.NotFound)
            {
                TempData["message"] = result.Message;
                return Redirect("Index");
            }
            _commerceCategoryService.ReInitManageModel(result.Result);
            return View("Manage", result.Result);
        }

        [HttpPost]
        public ActionResult Manage(CommerceCategoryManageModel model)
        {
            TempData["CurrentMenu"] = "CommerceCategory";
            if (ModelState.IsValid)
            {
                var result = _commerceCategoryService.Manage(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            // Rebind model
            _commerceCategoryService.ReInitManageModel(model);

            return View("Manage", model);
        }
        
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceCategory, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "CommerceCategory";
            TempData["message"] = _commerceCategoryService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
