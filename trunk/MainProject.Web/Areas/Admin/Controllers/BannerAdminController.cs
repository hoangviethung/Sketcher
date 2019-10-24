using MainProject.Core.Enums;
using MainProject.Web.BaseControllers;
using System.Web.Mvc;
using MainProject.Framework.ActionFilters;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Service.Banner;
using MainProject.ServiceAdmin.Model.Banner;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class BannerAdminController : BaseController
    {
        private readonly BannerService _bannerService;

        public BannerAdminController()
        {
            _bannerService = new BannerService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBanner, Permission = PermissionCollection.View)]
        public ActionResult Index(int languageId = 0, int page = 1)
        {
            TempData["CurrentMenu"] = "BannerAdmin";
            return View(_bannerService.GetIndex(languageId, page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBanner, Permission = PermissionCollection.Create)]
        public ActionResult Create(int id = 0)
        {
            TempData["CurrentMenu"] = "BannerAdmin";
            return View(_bannerService.Create());
        }

        [HttpPost]
        public ActionResult Create(BannerManageViewModel model)
        {
            TempData["CurrentMenu"] = "BannerAdmin";
            if (ModelState.IsValid)
            {
                _bannerService.Insert(model);
                TempData["message"] = string.Format("<strong style='color:green'>Thêm mới banner thành công!</strong>");
                return RedirectToAction("Index");
            }
            _bannerService.BindSelectListItem(model);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBanner, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "BannerAdmin";
            var model = _bannerService.Edit(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [HttpPost]
        public ActionResult Edit(BannerManageViewModel model)
        {
            TempData["CurrentMenu"] = "BannerAdmin";
            if (ModelState.IsValid)
            {
                var result = _bannerService.Update(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index", new { LanguageId = model.LanguageSelectedValue });
                }
            }
            _bannerService.BindSelectListItem(model);
            return View(model);
        }

        //Delete
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBanner, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "BannerAdmin";
            TempData["message"] = _bannerService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}