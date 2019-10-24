using System.Web.Mvc;
using MainProject.Web.BaseControllers;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Service.Upload;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Upload;

namespace MainProject.Web.Areas.Admin.Controllers
{
    [FilterLogonPermission]
    public class UploadController : BaseController
    {
        private readonly UploadService _uploadService;

        public UploadController()
        {
            _uploadService = new UploadService(DbContext, WebSecurity.CurrentUserName);
        }

        public ActionResult Images(string folder)
        {
            return View(_uploadService.GetImages(folder));
        }

        [HttpPost]
        [Route("delete-image")]
        public ActionResult DeleteImage(ImageInfo model)
            => Json(_uploadService.DeleteImage(model));

        [HttpPost]
        [ActionName("Files")]
        public JsonResult Files_Post(string UrlFolder)
        {
            return Json(_uploadService.Files_Post(UrlFolder, Request).ToArray(), "text/html", System.Text.Encoding.UTF8);
        }

        public ActionResult UpdateImage(string folder)
        {
            ViewBag.Images = _uploadService.UpdateImage(folder);
            return PartialView("_ImagesTable", _uploadService.UpdateImage(folder));
        }
    }
}
