using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Service.User;
using MainProject.Web.BaseControllers;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.User;
using MainProject.Framework.Constant;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class UserAdminController : BaseMembershipController
    {
        private readonly UserService _service;

        public UserAdminController()
        {
            _service = new UserService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageUsers, Permission = PermissionCollection.View)]
        public ActionResult Index(string userName = "", int page = 1)
        {
            TempData["CurrentMenu"] = "User";
            return View(_service.GetIndex(userName, page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageUsers, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            TempData["CurrentMenu"] = "User";
            return View(_service.Create());
        }

        [HttpPost]
        public ActionResult Create(UserManageCreateViewModel model)
        {
            TempData["CurrentMenu"] = "User";
            var result = _service.Insert(model);
            if (ModelState.IsValid)
            {
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", result.Message);
            }
            _service.RebindSelectListItem(model);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageUsers, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(int id)
        {
            TempData["CurrentMenu"] = "User";
            var model = _service.Edit(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [HttpPost]
        public ActionResult Edit(UserManageEditViewModel model)
        {
            TempData["CurrentMenu"] = "User";
            var result = _service.UpdateProfile(model);

            if (result.Code == HttpStatusCodeCollection.OK)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageUsers, Permission = PermissionCollection.Edit)]
        public ActionResult ChangePassword(int id)
        {
            TempData["CurrentMenu"] = "User";
            var model = _service.EditPassword(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [HttpPost]
        public ActionResult ChangePassword(UserManagePasswordModel model)
        {
            TempData["CurrentMenu"] = "User";

            var result = _service.UpdatePassword(model);

            if (result.Code == HttpStatusCodeCollection.OK)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageUsers, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "User";
            TempData["message"] = _service.Delete(id, User.Identity.Name);
            return RedirectToAction("Index");
        }
    }
}
