using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Setting;
using MainProject.ServiceAdmin.Service.Setting;
using MainProject.Web.BaseControllers;
using WebMatrix.WebData;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class SettingAdminController : BaseController
    {
        private SettingService _settingService;

        public SettingAdminController()
        {
            _settingService = new SettingService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageSettings, Permission = PermissionCollection.View)]
        public ActionResult Index()
        {
            TempData["CurrentMenu"] = "Setting";
            return View(_settingService.GetIndex());
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageSettings, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(int id)
        {
            TempData["CurrentMenu"] = "Setting";
            var model = _settingService.Edit(id);
            if(model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [HttpPost]
        public ActionResult Edit(SettingManageViewModel model)
        {
            TempData["CurrentMenu"] = "Setting";
            if(ModelState.IsValid)
            {
                var result = _settingService.Update(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //public ActionResult Delete(int id)
        //{
        //    TempData["CurrentMenu"] = "Setting";
        //    TempData["message"] = _settingService.Delete(id);
        //    return RedirectToAction("Index");
        //}

    }
}
