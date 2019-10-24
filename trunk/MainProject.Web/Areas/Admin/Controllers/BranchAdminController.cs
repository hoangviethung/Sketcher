using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Branch;
using MainProject.ServiceAdmin.Service.Branch;
using MainProject.Web.BaseControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class BranchAdminController : BaseController
    {
        private readonly BranchService _branchService;

        public BranchAdminController()
        {
            _branchService = new BranchService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.View)]
        public ActionResult Index(int languageId = 0, int page = 1)
        {
            TempData["CurrentMenu"] = "BranchAdmin";
            return View(_branchService.GetIndex(languageId, page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Create)]
        public ActionResult Create(int id = 0)
        {
            TempData["CurrentMenu"] = "BranchAdmin";
            return View(_branchService.Create());
        }

        [HttpPost]
        public ActionResult Create(BranchManageViewModel model)
        {
            TempData["CurrentMenu"] = "BranchAdmin";
            if (ModelState.IsValid)
            {
                _branchService.Insert(model);
                TempData["message"] = string.Format("<strong style='color:green'>Thêm mới chi nhánh thành công!</strong>");
                return RedirectToAction("Index", new { languageId = model.LanguageSelectedValue });
            }
            _branchService.BindSelectListItem(model);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "BranchAdmin";
            var model = _branchService.Edit(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [HttpPost]
        public ActionResult Edit(BranchManageViewModel model)
        {
            TempData["CurrentMenu"] = "BranchAdmin";
            if (ModelState.IsValid)
            {
                var result = _branchService.Update(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index", new { LanguageId = model.LanguageSelectedValue });
                }
            }
            _branchService.BindSelectListItem(model);
            return View(model);
        }

        //Delete
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "BranchAdmin";
            TempData["message"] = _branchService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}