using System;
using System.Web.Mvc;
using MainProject.Framework.Constant;
using MainProject.Web.BaseControllers;
using MainProject.Service.Service.Home;
using MainProject.Service.Model.Home;

namespace MainProject.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region
        private readonly HomeService _service;
        #endregion

        #region Constructors
        public HomeController()
        {
            _service = new HomeService(DbContext);
        }
        #endregion


        public ActionResult Index()
        {
            var model = _service.Get();

            if (model != null)
            {
                //Set page title
                ViewBag.Title = model.Category.Title;
                //Set Meta data
                ViewBag.MetaTitle = model.Category.MetaTitle;
                ViewBag.MetaDescription = model.Category.MetaDescription;
                ViewBag.MetaKeywords = model.Category.MetaKeywords;
                ViewBag.MetaImage = Request.Url.Scheme + "://" + Request.Url.Authority + model.Category.ImageDefault;
                ViewBag.Type = "website";
                ViewBag.Canonical = Request.Url.Scheme + "://" + Request.Url.Authority;
            }

            return View(model);
        }

        // Render the view of header
        public ActionResult ShowHeader()
            => View(_service.GetHeader(CurrentLanguageId));
			
		// Render the footer view at the right
        public ActionResult ShowFooter()
            => View(_service.GetFooter(CurrentLanguageId));

        public ActionResult Category(int id, string breadcumUrl)
        {
            var category = _service.GetCategory(id);
            if (category != null)
            {
                //if (category.DisplayTemplate == DisplayTemplateCollection.Achievement)
                //{
                //    return
                //        new MVCTransferResult(
                //            new
                //            {
                //                controller = "Achievement",
                //                action = "Index",
                //                id = category.Id
                //            });
                //}
            }
            return View(StringConstant.ViewForErrorPage);
        }

        public ActionResult Article(int id, string breadcumUrl)
        {
            var entity = _service.GetArticle(id);

            if (entity != null)
            {
                //Added by MK             
                //if (entity.Category.DisplayTemplate == DisplayTemplateCollection.IntroTemplate)
                //{
                //    return
                //        new MVCTransferResult(
                //            new
                //            {
                //                controller = "Intro",
                //                action = "Detail",
                //                id = id
                //            });
                //}
            }

            return View(StringConstant.ViewForErrorPage);
        }

        [Route("dang-ky-nhan-tin")]
        [HttpPost]
        public JsonResult Subscribe(SubscribeModel model)
            => Json(_service.Subscribe(ModelState.IsValid, model));

        public PartialViewResult Sitemap()
        {
            var list = _service.GetDataOfSitemap();
            Response.AddHeader("Content-Type", "text/xml");
            ViewBag.host = String.Format("http://{0}", Request.Url.Host);
            return PartialView(list);
        }

        public ActionResult Error()
            => View(StringConstant.ViewForErrorPage);

        public ActionResult Maintainance() => View();
    }
}

