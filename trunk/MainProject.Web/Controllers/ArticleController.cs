using MainProject.Service.Service.Article;
using MainProject.Web.BaseControllers;
using System.Web.Mvc;

namespace MainProject.Web.Controllers
{
    public class ArticleController : BaseController
    {
        // GET: Article
        private readonly ArticleService _service;

        public ArticleController()
        {
            _service = new ArticleService(DbContext);
        }

        public ActionResult Detai(long id)
        {
            var model = _service.GetDetail(id);

            if (model != null)
            {
                //Set page title
                ViewBag.Title = model.Title;
                //Set Meta data
                ViewBag.MetaTitle = model.MetaTitle;
                ViewBag.MetaDescription = model.MetaDescription;
                ViewBag.MetaKeywords = model.MetaKeywords;
                ViewBag.MetaImage = Request.Url.Scheme + "://" + Request.Url.Authority + model.ImageDefault;
                ViewBag.Type = "website";
                ViewBag.Canonical = Request.Url.Scheme + "://" + Request.Url.Authority;
            }

            return View(model);
        }
    }
}