//using MainProject.Web.BaseControllers;
//using System.Web.Mvc;

//namespace MainProject.Web.Controllers
//{
//    public class FaqController : BaseController
//    {
//        private readonly FaqService _faqService;

//        public FaqController()
//        {
//            _faqService = new FaqService(DbContext);
//        }

//        /// <summary>
//        /// Frequently asked questions index page
//        /// </summary>
//        /// <param name="page">In case website use paging</param>
//        /// <returns>The question what admin answered</returns>
//        public ActionResult Index(int page = 1)
//        {
//            var model = _faqService.Get(CurrentLanguageId, page);

//            if (model != null)
//            {
//                //Set page title
//                ViewBag.Title = model.Category.Title;
//                //Set Meta data
//                ViewBag.MetaTitle = model.Category.MetaTitle;
//                ViewBag.MetaDescription = model.Category.MetaDescription;
//                ViewBag.MetaKeywords = model.Category.MetaKeywords;
//                ViewBag.MetaImage = Request.Url.Scheme + "://" + Request.Url.Authority + model.Category.ImageDefault;
//                ViewBag.Type = "website";
//                ViewBag.Canonical = Request.Url.Scheme + "://" + Request.Url.Authority;
//            }
//            return View(model);
//        }

//        /// <summary>
//        /// Load more the question what admin answered
//        /// </summary>
//        /// <param name="page">Current page</param>
//        /// <param name="IsMobile">For response html pc or mobile</param>
//        /// <returns></returns>
//        [Route("LoadFAQ")]
//        public JsonResult LoadFAQ(int page = 2, bool IsMobile = false)
//        {
//            var str = "";
//            bool isCanLoadMore = false;
//            _faqService.Load(ref str, ref isCanLoadMore, page, IsMobile);

//            return Json(new { html = str, IsCanLoadMore = isCanLoadMore }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult FaqQuestion(FaqQuestionViewModel model)
//            => Json(new { result = _faqService.Save(model) });
//    }
//}