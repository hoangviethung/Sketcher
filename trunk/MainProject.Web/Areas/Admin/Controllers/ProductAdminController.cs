using MainProject.Core.Enums;
using System.Web.Mvc;
using MainProject.Framework.ActionFilters;
using MainProject.Web.BaseControllers;
using MainProject.ServiceAdmin.Service.Product;
using WebMatrix.WebData;
using MainProject.ServiceAdmin.Model.Product;
using System.Linq;

namespace MainProject.Web.Areas.Admin.Controllers
{
    public class ProductAdminController : BaseController
    {
        private readonly ProductService _productService;

        public ProductAdminController()
        {
            _productService = new ProductService(DbContext);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProduct, Permission = PermissionCollection.View)]
        public ActionResult Index(long fa = 0, string title = null, int page = 1)
        {
            TempData["CurrentMenu"] = "Product";
            return View(_productService.GetIndex(fa, title,  page));
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProduct, Permission = PermissionCollection.Create)]
        public ActionResult Create()
        {
            TempData["CurrentMenu"] = "Product";  
            return View(_productService.Create());
        }

        public ActionResult CreateProperty(int pos, long[] selectedIds)
            => View("_Property", _productService.CreateProperty(pos, selectedIds.ToList()));

        [HttpPost]
        public ActionResult Create(ProductManageModel model)
        {
            TempData["CurrentMenu"] = "Product";
            if (ModelState.IsValid)
            {
                var result = _productService.Insert(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = result.Message;
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProduct, Permission = PermissionCollection.Edit)]
        public ActionResult Edit(long id)
        {
            TempData["CurrentMenu"] = "Product";
            var res = _productService.Edit(id);
            if(res.Code == HttpStatusCodeCollection.BadRequest)
            {
                TempData["message"] = res.Message;
                return RedirectToAction("Index");
            }
            return View(res.Result);
        }

        [HttpPost]
        public ActionResult Edit(ProductManageModel model)
        {
            TempData["CurrentMenu"] = "Product";
            if (ModelState.IsValid)
            {
                var result = _productService.Update(model);
                if (result.Code == HttpStatusCodeCollection.OK)
                {
                    TempData["message"] = string.Format("<strong style='color:green'>Cập nhật thành công!</strong>");
                    return RedirectToAction("Index");
                }    
            }
            // In case has some error and return data to view again, we should rebind select list model here

            return View(model);
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProduct, Permission = PermissionCollection.Delete)]
        public ActionResult Delete(long id)
        {
            TempData["CurrentMenu"] = "Product";
            TempData["message"] = _productService.Delete(id);
            return RedirectToAction("Index");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProduct, Permission = PermissionCollection.Edit)]
        public ActionResult ManageBlock(int id, bool isBlock)
        {
            TempData["CurrentMenu"] = "Product";
            TempData["message"] = _productService.ManageLock(id, isBlock);
            return RedirectToAction("Index");
        }

        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageCommerceProduct, Permission = PermissionCollection.Edit)]
        public ActionResult ManageHide(int id, bool isHide)
        {
            TempData["CurrentMenu"] = "Product";
            TempData["message"] = _productService.ManageHide(id, isHide);
            return RedirectToAction("Index");
        }
    }
}
