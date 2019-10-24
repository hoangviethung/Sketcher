using System.Reflection;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Core.UserInfos;
using MainProject.Data.Repositories;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Role;
using MainProject.ServiceAdmin.Service.Role;
using MainProject.Web.BaseControllers;
using WebMatrix.WebData;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class RoleAdminController : BaseController
    {
        private readonly RoleRepository _roleRepository;
        private readonly RoleService _roleService;

        public RoleAdminController()
        {
            _roleRepository = new RoleRepository(DbContext);
            _roleService = new RoleService(DbContext,WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManagePermissions, Permission = PermissionCollection.View)]
        public ActionResult Index()
        {
            //set current active menu of admin page
            TempData["CurrentMenu"] = "roleadmin";
            return View(_roleService.GetIndex());
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManagePermissions, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            return RedirectToAction("Manage");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManagePermissions, Permission = PermissionCollection.Edit)]
        public ActionResult Manage(int? id)
        {
            //set current active menu of admin page
            TempData["CurrentMenu"] = "roleadmin";
            return View(_roleService.Manage(id));
        }

        [HttpPost]
        public ActionResult Manage(RoleManageModel model)
        {
            if (ModelState.IsValid)
            {
                _roleService.Manage(model);
                TempData["message"] = string.Format("<strong style='color:green'>Cập nhật nhóm quyền thành công!</strong>");
                return RedirectToAction("Index");
            }

            model.BuildActiveFeatures(Assembly.GetExecutingAssembly());
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManagePermissions, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["message"] = string.Format(_roleService.Delete(id));    
            return RedirectToAction("Index");
        }       
    }
}