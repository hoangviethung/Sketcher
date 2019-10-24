using System.Web.Mvc;
using MainProject.Web.BaseControllers;
using MainProject.ServiceAdmin.Service.Common;
using MainProject.Framework.ActionFilters;

namespace MainProject.Web.Areas.Admin.Controllers
{
    [FilterLogonPermission]
    public class AdminCommonController : BaseController
    {
        private readonly CommonService _commonService;

        public AdminCommonController()
        {
            _commonService = new CommonService(DbContext);
        }

        public JsonResult GetCategoryByLanague(int id, long cateId = 0)
            => Json(new { Data = _commonService.GetCategories(id, cateId) }, JsonRequestBehavior.AllowGet);

        public JsonResult GetMenuByLanguage(int langId, int selectedValue = 0)
            => Json(new { Data = _commonService.GetMenus(langId, selectedValue) }, JsonRequestBehavior.AllowGet);

        public JsonResult GetMenuItems(int langId, int menuId, int selectedValue = 0)
            => Json(new { Data = _commonService.GetMenuItems(langId, menuId, selectedValue) }, JsonRequestBehavior.AllowGet);

        public JsonResult GetNewsCategoryByLanguage(int langId, long selectedValue = 0)
            => Json(new { Data = _commonService.GetCategories(langId, selectedValue) }, JsonRequestBehavior.AllowGet);
    }
}
