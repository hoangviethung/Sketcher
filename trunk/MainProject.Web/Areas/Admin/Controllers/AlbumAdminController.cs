using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Album;
using MainProject.ServiceAdmin.Service.Album;
using MainProject.Web.BaseControllers;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class AlbumAdminController : BaseController
    {
        private readonly AlbumService _service;

        public AlbumAdminController()
        {
            _service = new AlbumService(DbContext, WebSecurity.CurrentUserName);
        }

        // GET: Admin/Album

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageGallery, Permission = PermissionCollection.View)]
        public ActionResult Index(int languageId = 0, int page = 1)
        {
            TempData["CurrentMenu"] = "AlbumAdmin";
            return View(_service.GetIndex(languageId, page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageGallery, Permission = PermissionCollection.Create)]
        public ActionResult Create(int id = 0)
        {
            TempData["CurrentMenu"] = "AlbumAdmin";
            return View(_service.Create());
        }

        public ActionResult CreateMedia(int pos)
            => PartialView("_Media", new MediaManageViewModel() { Position = pos });

        [HttpPost]
        public ActionResult Create(GalleryManageViewModel model)
        {
            TempData["CurrentMenu"] = "AlbumAdmin";
            if (ModelState.IsValid)
            {
                _service.Insert(model);
                TempData["message"] = string.Format("<strong style='color:green'>Thêm mới banner thành công!</strong>");
                return RedirectToAction("Index");
            }
            // In case some error's occurred, need to rebind model
            //_service.BindSelectListItem(model);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageGallery, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "AlbumAdmin";
            var model = _service.Edit(id);
            if (model.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = model.Message;
                return RedirectToAction("Index");
            }
            return View(model.Result);
        }

        [HttpPost]
        public ActionResult Edit(GalleryManageViewModel model)
        {
            TempData["CurrentMenu"] = "AlbumAdmin";
            if (ModelState.IsValid)
            {
                var result = _service.Update(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index", new { LanguageId = model.LanguageSelectedValue });
                }
            }
            // In case some error's occurred, need to rebind model
            //_service.BindSelectListItem(model);
            return View(model);
        }

        //Delete
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageGallery, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "AlbumAdmin";
            TempData["message"] = _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}