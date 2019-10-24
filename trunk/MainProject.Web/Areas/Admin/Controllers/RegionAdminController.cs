using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Region;
using MainProject.ServiceAdmin.Service.Region;
using MainProject.Web.BaseControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class RegionAdminController : BaseController
    {
        private readonly RegionService _service;

        public RegionAdminController()
        {
            _service = new RegionService(DbContext, WebSecurity.CurrentUserName);
        }

        // GET: Admin/Region
        #region City
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.View)]
        public ActionResult CityIndex(int page = 0)
        {
            TempData["CurrentMenu"] = "CityAdmin";
            return View(_service.GetCity(page));
        }

        [HttpPost]
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Create)]
        public ActionResult Create(CityManageViewModel model)
        {
            TempData["CurrentMenu"] = "CityAdmin";
            TempData["message"] = _service.InsertCity(model).Message;
            return RedirectToAction("CityIndex");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Edit)]
        public ActionResult CityEdit(int id)
        {
            TempData["CurrentMenu"] = "CityAdmin";
            var result = _service.EditCity(id);
            if (result.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("CityIndex");
            }

            return View(result.Result);
        }

        [HttpPost]
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Edit)]
        public ActionResult CityEdit(CityManageViewModel model)
        {
            TempData["CurrentMenu"] = "CityAdmin";
            TempData["message"] = _service.UpdateCity(model).Message;
            return RedirectToAction("CityIndex");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Delete)]
        public ActionResult CityDelete(int id)
        {
            TempData["CurrentMenu"] = "CityAdmin";
            TempData["message"] = _service.DeleteCity(id).Message;
            return RedirectToAction("CityIndex");
        } 
        #endregion

        #region District
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.View)]
        public ActionResult DistrictIndex(int city = 0, int page = 0)
        {
            TempData["CurrentMenu"] = "DistrictAdmin";
            return View(_service.GetDistrict(city, page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Create)]
        public ActionResult DistrictCreate()
        {
            TempData["CurrentMenu"] = "DistrictAdmin";
            return View(_service.CreateDistrict());
        }

        [HttpPost]
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Create)]
        public ActionResult DistrictCreate(DisctrictManageViewModel model)
        {
            TempData["CurrentMenu"] = "DistrictAdmin";
            TempData["message"] = _service.InsertDisctrict(model).Message;
            return RedirectToAction("DistrictIndex");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Edit)]
        public ActionResult DistrictEdit(int id)
        {
            TempData["CurrentMenu"] = "DistrictAdmin";
            var result = _service.EditDisctrict(id);
            if (result.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("DistrictIndex");
            }

            return View(result.Result);
        }

        [HttpPost]
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Edit)]
        public ActionResult DistrictEdit(DisctrictManageViewModel model)
        {
            TempData["CurrentMenu"] = "DistrictAdmin";
            TempData["message"] = _service.UpdateDisctrict(model).Message;
            return RedirectToAction("DistrictIndex");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageBranch, Permission = PermissionCollection.Delete)]
        public ActionResult DistrictDelete(int id)
        {
            TempData["CurrentMenu"] = "DistrictAdmin";
            TempData["message"] = _service.DeleteCity(id).Message;
            return RedirectToAction("DistrictIndex");
        }
        #endregion

        #region Common Api
        [Route("get-city")]
        public ActionResult GetCity()
            => Json(_service.GetCityApi(), JsonRequestBehavior.AllowGet);

        [Route("get-district")]
        public ActionResult GetDistrict(int cityId, int id = 0)
            => Json(_service.GetDistrictApi(cityId, id), JsonRequestBehavior.AllowGet);

        [Route("get-branch")]
        public ActionResult GetBranch(int cityId, int id = 0, List<int> oilType = default(List<int>), string address = "")
            => Json(_service.GetBranchApi(cityId, oilType, id, address), JsonRequestBehavior.AllowGet);
        #endregion
    }
}