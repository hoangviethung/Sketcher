//using System;
//using System.Web.Mvc;
//using MainProject.Core.Enums;
//using MainProject.Web.BaseControllers;
//using MainProject.Framework.ActionFilters;
//using MainProject.ServiceAdmin.Service.FaqItem;
//using WebMatrix.WebData;
//using MainProject.ServiceAdmin.Model.FaqItem;

//namespace MainProject.Web.Areas.Admin.Controllers
//{
//    public class FaqItemAdminController : BaseController
//    { 
//        private FaqItemService _faqItemService;
//        public FaqItemAdminController()
//        {
//            _faqItemService = new FaqItemService(DbContext, WebSecurity.CurrentUserName);
//        }

//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaq, Permission = PermissionCollection.View)]
//        public ActionResult Index(int status = (int)FaqApprovalStatusCollection.NotPending, string title = null, int page = 1)
//        {
//            TempData["CurrentMenu"] = "FaqItemAdmin";           
//            return View(_faqItemService.GetIndex(status, title, page));
//        }

//        //[AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaqItem, Permission = PermissionCollection.View)]
//        //public ActionResult Detail(long id)
//        //{
//        //    TempData["CurrentMenu"] = "FaqItemAdmin";
//        //    var model = _faqItemService.GetDetail(id, WebSecurity.CurrentUserName);
//        //    if(model != null)
//        //    {
//        //        return View(model);
//        //    }
//        //    TempData["message"] = string.Format("<strong style='color:red'>Không tìm được đối tượng cần xem</strong>");
//        //    return RedirectToAction("Index");
//        //}

//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaq, Permission = PermissionCollection.Edit)]
//        public ActionResult Answer(long id)
//        {
//            TempData["CurrentMenu"] = "FaqItemAdmin";
//            var result = _faqItemService.Answer(id);
//            if (result.Code != HttpStatusCodeCollection.BadRequest)
//            {      
//                return View(result.Result);
//            }

//            TempData["message"] = result.Message;
//            return RedirectToAction("Index");
//        }

//        [HttpPost]
//        public ActionResult Answer(FaqItemAnswerManageModel faqItemAnswerManageModel)
//        {
//            TempData["CurrentMenu"] = "FaqItemAdmin";
//            if (ModelState.IsValid)
//            {
//                _faqItemService.Answer(faqItemAnswerManageModel, WebSecurity.CurrentUserName);
//                TempData["message"] = string.Format("<strong style='color:green'>trả lời thành công</strong>");
//                return RedirectToAction("Index");
//            }
//            return View(faqItemAnswerManageModel);
//        }

//        //[AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaqItem, Permission = PermissionCollection.Edit)]
//        //public ActionResult Edit(long id)
//        //{
//        //    TempData["CurrentMenu"] = "FaqItemAdmin";
//        //    var model = _faqItemService.Eidt(id);
//        //    if (model != null)
//        //    {   
//        //        return View(model);
//        //    }
//        //    TempData["message"] = string.Format("<strong style='color:red'>Không tìm thấy câu hỏi</strong>");
//        //    return RedirectToAction("Index");
//        //}

//        //[HttpPost]
//        //public ActionResult Edit(FaqItemAnswerManageModel faqItemAnswerManageModel)
//        //{
//        //    TempData["CurrentMenu"] = "FaqItemAdmin";
//        //    if (ModelState.IsValid)
//        //    {
//        //        _faqItemService.Eidt(faqItemAnswerManageModel, WebSecurity.CurrentUserName);
//        //        TempData["message"] = string.Format("<strong style='color:green'>đã được cập nhật thành công</strong>");
//        //        return RedirectToAction("Index");
//        //    }
//        //    return View(faqItemAnswerManageModel);
//        //}

//        //[AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaqItem, Permission = PermissionCollection.Edit)]
//        //public ActionResult Approve(long id)
//        //{
//        //    TempData["CurrentMenu"] = "FaqItemAdmin";  
//        //    var res = _faqItemService.Approve(id, WebSecurity.CurrentUserName);
//        //    if (res.Result)
//        //    {         
//        //        return Json(new { Result = res.Result, Message = res.Message, CallBack = res.CallBack }, JsonRequestBehavior.AllowGet);
//        //    }
//        //    TempData["message"] = string.Format("<strong style='color:red'>" + res.Message+ "</strong>"); ;
//        //    return RedirectToAction("Index");
//        //}

//        //[AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaqItem, Permission = PermissionCollection.Edit)]
//        //public ActionResult Reject(long id)
//        //{
//        //    TempData["CurrentMenu"] = "FaqItemAdmin";
//        //    var res = _faqItemService.Reject(id, WebSecurity.CurrentUserName);
//        //    if(res.Result)
//        //    {
//        //        return Json(new { Result = res.Result, Message = res.Message, CallBack = res.CallBack }, JsonRequestBehavior.AllowGet);
//        //    }
//        //    TempData["message"] = string.Format("<strong style='color:red'>" + res.Message + "</strong>");
//        //    return RedirectToAction("Index");
//        //}

//        //[AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaqItem, Permission = PermissionCollection.View)]
//        //public ActionResult Next(long id)
//        //{
//        //    TempData["CurrentMenu"] = "FaqItemAdmin";
//        //    try
//        //    {
//        //        return RedirectToAction("Detail", "FaqItemAdmin", new { @id = _faqItemService.Next(id) });
//        //    }
//        //    catch (Exception)
//        //    {
//        //        TempData["message"] = "<strong style='color:red'>Không còn kết quả để xem</strong>";
//        //        return RedirectToAction("Detail", "FaqItemAdmin", new { id = id });
//        //    }
//        //}

//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageFaq, Permission = PermissionCollection.Delete)]
//        public ActionResult Delete(int id)
//        {
//            TempData["CurrentMenu"] = "FaqItemAdmin";
//            TempData["message"] = _faqItemService.Delete(id);     
//            return RedirectToAction("Index");
//        }
//    }
//}
