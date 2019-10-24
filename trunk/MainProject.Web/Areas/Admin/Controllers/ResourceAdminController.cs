using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.Web.BaseControllers;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.StringResource;
using MainProject.ServiceAdmin.Service.StringResource;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class ResourceAdminController : BaseController
    {
        private readonly StringResourceService _stringResourceService;

        public ResourceAdminController()
        {
            _stringResourceService = new StringResourceService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageResource, Permission = PermissionCollection.View)]
        public ActionResult Index(int languageId = 0, string filter = null, int page = 1)
        {
            TempData["CurrentMenu"] = "Resource";
            return View(_stringResourceService.GetIndex(languageId, filter, page));
        }

        //public void InitResources()
        //{
        //    _stringResourceService.InitResources();
        //}

        //private void AddResourceKeyAndValues(string resourceName)
        //{
        //    _stringResourceService.AddResourceKeyAndValues(resourceName);
        //}

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageResource, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(int id)
        {
            TempData["CurrentMenu"] = "Resource";
            var resource = _stringResourceService.Edit(id);
            if (resource != null)
            {
                return View(resource);
            }
            TempData["message"] = string.Format("<span style='color:red'>Không tìm thấy đối tượng cần sửa</span>");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(StringResourceValueModel model)
        {
            TempData["CurrentMenu"] = "Resource";
            if(ModelState.IsValid)
            {
                _stringResourceService.Update(model);
                TempData["message"] = string.Format("<span style='color:green'>{0} đã được cập nhật</span>", model.KeyName);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
