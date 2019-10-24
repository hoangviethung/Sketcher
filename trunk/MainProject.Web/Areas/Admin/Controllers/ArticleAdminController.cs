using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Framework.ActionFilters;
using MainProject.ServiceAdmin.Model.Article;
using MainProject.ServiceAdmin.Service.Article;
using MainProject.Web.BaseControllers;
using WebMatrix.WebData;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class ArticleAdminController : BaseController
    {     
        private readonly ArticleService _articleService;

        public ArticleAdminController()
        {
            _articleService = new ArticleService(DbContext, WebSecurity.CurrentUserName);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageNews, Permission = PermissionCollection.View)]
        public ActionResult Index(string text, int cul = 0, long fa = 0, int page = 1)
        {
            //set current active menu of admin page
            TempData["CurrentMenu"] = "ArticleAdmin";
            return View(_articleService.GetIndex(text, cul, fa, page));
        }

        // id !=0 -> add language version for article 
        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageNews, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            TempData["CurrentMenu"] = "ArticleAdmin";
            return View(_articleService.Create());
        }

        [HttpPost]
        public ActionResult Create(ArticleManageViewModel model)
        {
            TempData["CurrentMenu"] = "ArticleAdmin";
            if (ModelState.IsValid)
            {
                _articleService.Insert(model);
                TempData["message"] = string.Format("<strong style='color:green'>Thành công!</strong>");
                return RedirectToAction("Index", new { cul = model.LanguageSelectedValue });         
            }

            _articleService.BindSelectListItem(model);
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageNews, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(int id)
        {
            TempData["CurrentMenu"] = "ArticleAdmin";
            var result = _articleService.Edit(id);
            if (result. Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }

            return View(result.Result);
        }

        [HttpPost]
        public ActionResult Edit(ArticleManageViewModel model)
        {
            TempData["CurrentMenu"] = "ArticleAdmin"; 
            if (ModelState.IsValid)
            {
                var result = _articleService.Update(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index", new { cul = model.LanguageSelectedValue, fa = model.CategorySelectedValue});
                }
            }
            _articleService.BindSelectListItem(model);

            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageNews, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(int id)
        {
            TempData["CurrentMenu"] = "ArticleAdmin";
            var result = _articleService.Delete(id);
            TempData["message"] = result.Message;

            if (result.Code == HttpStatusCodeCollection.BadRequest)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { cul = result.Result });
        }
    }
}
