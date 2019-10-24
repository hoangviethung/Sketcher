using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.Web.BaseControllers;
using MainProject.ServiceAdmin.Service.Category;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.Category;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class CategoryAdminController : BaseController
    {
        private readonly CategoryService _service;

        public CategoryAdminController()
        {
            _service = new CategoryService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCategories, Permission = PermissionCollection.View)]
        public ActionResult Index(int languageId = 0, int page = 1)
        {
            TempData["CurrentMenu"] = "CategoryAdmin";
            return View(_service.GetIndex(languageId, page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCategories, Permission = PermissionCollection.Create)]
        public ActionResult Create(int id = 0)
        {
            TempData["CurrentMenu"] = "CategoryAdmin";
            return View(_service.Create());
        }

        [HttpPost]
        public ActionResult Create(CategoryManageViewModel model)
        {
            TempData["CurrentMenu"] = "CategoryAdmin";
            if (ModelState.IsValid)
            {
                _service.Insert(model);

                TempData["message"] = string.Format("<strong style='color:green'>Tạo mới danh mục thành công!</strong>");
                return RedirectToAction("Index");
            }
            _service.BindSelectListItem(model);

            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCategories, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "CategoryAdmin";
            var model = _service.Edit(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                return RedirectToAction("Index");
            }

            return View(model.Result);
        }

        [HttpPost]
        public ActionResult Edit(CategoryManageViewModel model)
        {
            TempData["CurrentMenu"] = "CategoryAdmin";
            if (ModelState.IsValid)
            {
                // Check whether update category data is successful
                if (_service.Update(model))
                {
                    TempData["message"] = string.Format("<strong style='color:green'>Chỉnh sửa danh mục thành công!</strong>");
                    return RedirectToAction("Index", new { LanguageId = model.LanguageSelectedValue });
                }
            }
            _service.BindSelectListItem(model);

            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCategories, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "CategoryAdmin";

            TempData["message"] = _service.Delete(id);
            return RedirectToAction("Index");

        }
    }
}