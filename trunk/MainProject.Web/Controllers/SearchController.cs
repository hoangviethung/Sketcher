using MainProject.Service.Service.Search;
using MainProject.Web.BaseControllers;
using System.Web.Mvc;

namespace MainProject.Web.Controllers
{
    public class SearchController : BaseController
    {
        private readonly SearchService _service;
        public SearchController()
        {
            _service = new SearchService(DbContext);
        }

        [Route("search")]
        public ActionResult Index(string text="", int page=1)
        {
            
            return View();
        }
    }
}