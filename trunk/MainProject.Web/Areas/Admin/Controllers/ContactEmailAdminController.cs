//using MainProject.Core;
//using MainProject.Core.Enums;
//using MainProject.Data.Repositories;
//using MainProject.Framework.ActionFilters;
//using MainProject.Framework.Models;
//using MainProject.Web.Areas.Admin.Helpers;
//using MainProject.ServiceAdmin.Model;
//using MainProject.ServiceAdmin.Model.Album;
//using MainProject.ServiceAdmin.Model.ContactEmail;
//using MainProject.Web.BaseControllers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using WebMatrix.WebData;

//namespace MainProject.Web.Areas.Admin.Controllers
//{
//    public class ContactEmailAdminController : BaseController
//    {
//        // GET: Admin/AlbumAdmin
//        private static readonly int PAGE_ITEMS = 15;
//        private readonly LanguageRepository _languageRepository;
//        private readonly ContactEmailRepository _contactEmailRepository;
//        private readonly LogHistoryRepository _logHistoryRepository;

//        private string CurrentUser
//        {
//            get { return HttpContext.User.Identity.Name; }
//        }

//        public ContactEmailAdminController()
//        {
//            _languageRepository = new LanguageRepository(DbContext);
//            _contactEmailRepository = new ContactEmailRepository(DbContext);
//            _logHistoryRepository = new LogHistoryRepository(DbContext);
//        }

//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContactEmails, Permission = PermissionCollection.View)]
//        public ActionResult Index(int page = 1)
//        {
//            TempData["CurrentMenu"] = "ContactEmailAdmin";
//            if (page < 1) page = 1;
//            var sql = _contactEmailRepository.FindAll();

//            int count = sql.Count();
//            var emails = sql.OrderBy(d => d.Id).Skip((page - 1) * PAGE_ITEMS).Take(PAGE_ITEMS).ToList();
//            var model = new IndexViewModel<ContactEmail>()
//            {
//                ListItems = emails,
//                PagingViewModel = new PagingModel(count, PAGE_ITEMS, page, "href='/Admin/ContactEmailAdmin/Index?&page={0}'")

//            };

//            return View(model);
//        }

//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContactEmails, Permission = PermissionCollection.Create)]
//        public ActionResult Create()
//        {
//            TempData["CurrentMenu"] = "ContactEmailAdmin";
//            var model = new ContactEmailManageModel() {
//                IsActive = true
//            };

//            return View(model);
//        }

//        [HttpPost]
//        public ActionResult Create(ContactEmailManageModel model)
//        {
//            TempData["CurrentMenu"] = "ContactEmailAdmin";
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var entity = new ContactEmail();
//                    ContactEmailManageModel.ToEntity(model, ref entity);
//                    _contactEmailRepository.Insert(entity);
//                    _contactEmailRepository.SaveChanges();

//                    var logHistory = new LogHistory()
//                    {
//                        EntityId = entity.Id,
//                        ActionBy = CurrentUser,
//                        ActionType = ActionTypeCollection.Create,
//                        CreatedDate = DateTime.Now,
//                        EntityType = EntityTypeCollection.Email,
//                        Comment = "create contact email"
//                    };
//                    _logHistoryRepository.Insert(logHistory);
//                    _logHistoryRepository.SaveChanges();

//                    TempData["message"] = string.Format("<strong style='color:green'>Thêm mới thành công!</strong>");
//                    return RedirectToAction("Index");
//                }
//                catch
//                {
//                    TempData["message"] = string.Format("<strong style='color:red'>Xảy ra lỗi!</strong>");
//                }

//            }

//            return View(model);
//        }


//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContactEmails, Permission = PermissionCollection.Edit)]
//        public ActionResult Edit(long id)
//        {
//            TempData["CurrentMenu"] = "ContactEmailAdmin";
//            var entity = _contactEmailRepository.FindUnique(c => c.Id == id);
//            var model = new ContactEmailManageModel(entity);

//            return View(model);
//        }

//        [HttpPost]
//        public ActionResult Edit(ContactEmailManageModel model)
//        {
//            TempData["CurrentMenu"] = "ContactEmailAdmin";
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    var entity = _contactEmailRepository.FindUnique(x => x.Id == model.Id);
//                    ContactEmailManageModel.ToEntity(model, ref entity);
//                    _contactEmailRepository.SaveChanges();

//                    var logHistory = new LogHistory()
//                    {
//                        EntityId = entity.Id,
//                        ActionBy = CurrentUser,
//                        ActionType = ActionTypeCollection.Edit,
//                        CreatedDate = DateTime.Now,
//                        EntityType = EntityTypeCollection.Email,
//                        Comment = "edit contact email"
//                    };
//                    _logHistoryRepository.Insert(logHistory);
//                    _logHistoryRepository.SaveChanges();

//                    TempData["message"] = string.Format("<strong style='color:green'>Chỉnh sửa thành công!</strong>");
//                    return RedirectToAction("Index");
//                }
//                catch
//                {
//                    TempData["message"] = string.Format("<strong style='color:red'>Xảy ra lỗi!</strong>");
//                }

//            }
            
//            return View(model);
//        }

//        [AuthorizeUserAttribute(Feature = EntityManageTypeCollection.ManageContactEmails, Permission = PermissionCollection.Delete)]
//        public ActionResult Delete(int Id)
//        {
//            TempData["CurrentMenu"] = "ContactEmailAdmin";

//            var entity = _contactEmailRepository.FindUnique(x => x.Id == Id);
//            if (entity == null) return RedirectToAction("Index");

//            _contactEmailRepository.Insert(entity);
//            _contactEmailRepository.SaveChanges();

//            var logHistory = new LogHistory()
//            {
//                EntityId = entity.Id,
//                ActionBy = CurrentUser,
//                ActionType = ActionTypeCollection.Delete,
//                CreatedDate = DateTime.Now,
//                EntityType = EntityTypeCollection.Email,
//                Comment = "delete contact email"
//            };
//            _logHistoryRepository.Insert(logHistory);
//            _logHistoryRepository.SaveChanges();

//            TempData["message"] = string.Format("<strong style='color:green'>Đã xóa thành công!</strong>");
//            return RedirectToAction("Index");
//        }
//    }
//}