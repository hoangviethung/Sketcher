using System.Web.Mvc;
using MainProject.Web.BaseControllers;
using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Service.Menu;
using MainProject.ServiceAdmin.Model.Menu;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class MenuAdminController : BaseController
    { 
        private readonly MenuService _menuService;

        public MenuAdminController()
        {
            _menuService = new MenuService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageMenu, Permission = PermissionCollection.View)]
        public ActionResult Index(int menuSelectedId = 0, int languageSelectedId = 0)
        {
            TempData["CurrentMenu"] = "Menu";      
            return View(_menuService.GetIndex(menuSelectedId, languageSelectedId));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageMenu, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            TempData["CurrentMenu"] = "Menu";
            var res = _menuService.Create();
            if(res == null)
                ViewBag.CannotCreateVersion = true;
            return View("~/Areas/Admin/Views/MenuAdmin/Manage.cshtml", res);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageMenu, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "Menu";
            return View("~/Areas/Admin/Views/MenuAdmin/Manage.cshtml", _menuService.Edit(id));
        }

        [HttpPost]
        public ActionResult Manage(MenuItemManageModel model)
        {
            TempData["CurrentMenu"] = "Menu";
            if (ModelState.IsValid)
            {
                var result = _menuService.Manage(model);
                if (result)
                {
                    TempData["message"] = string.Format("<span style='color:green'>{0} đã được cập nhật</span>", model.Title);
                    return RedirectToAction("Index", new { menuSelectedId = model.MenuSelectedValue, languageSelectedId = model.LanguageSelectedValue });
                }
            }
            // Some error occurred, need to rebind select list model here
            model = _menuService.ReInitManageModel(model);
            return View("Manage", model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageMenu, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "Menu";
            var result = _menuService.Delete(id);
            TempData["message"] = result.Message;
            if (result.Code == HttpStatusCodeCollection.OK)
            {         
                return RedirectToAction("Index", new { menuSelectedId = result.Result[0], languageId = result.Result[0] });
            }
            return RedirectToAction("Index");

        }
    }
}
