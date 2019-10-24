//using System;
//using System.Linq;
//using System.Web.Mvc;
//using MainProject.Core;
//using MainProject.Data.Repositories;
//using MainProject.Web.BaseControllers;

//namespace MainProject.Web.Areas.Admin.Controllers
//{
//    public class LanguageAdminController : BaseController
//    {
//        private string CurrentUSer
//        {
//            get { return HttpContext.User.Identity.Name; } 
//        }

//        private LanguageRepository LanguageRepository { get; set; }
//        private LogHistoryRepository _logHistoryRepository;

//        public LanguageAdminController()
//        {
//            LanguageRepository = new LanguageRepository(DbContext);
//            _logHistoryRepository = new LogHistoryRepository(DbContext);
//        }

//        public ActionResult Index()
//        {
//            return View(LanguageRepository.FindAll().ToList());
//        }

//        public ActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Create(Language language)
//        {
//            if(ModelState.IsValid)
//            {
//                LanguageRepository.Insert(language);
//                // add cac string resource value tuong ung cho moi resource key cho language này
//                LanguageRepository.SaveChanges();
//                var logHistory = new LogHistory()
//                {
//                    EntityId = language.Id,
//                    EntityType = Core.Enums.EntityTypeCollection.Language,
//                    CreatedDate = DateTime.Now,
//                    ActionBy = CurrentUSer,
//                    ActionType = Core.Enums.ActionTypeCollection.Create,
//                    Comment = "create language"
//                };
//                _logHistoryRepository.Insert(logHistory);
//                return RedirectToAction("Index");
//            }
//            return View();
//        }

//        public ActionResult Edit(int id)
//        {
//            var model = LanguageRepository.FindId(id);
//            return View(model);
//        }

//        [HttpPost]
//        public ActionResult Edit(Language language)
//        {
//            if (ModelState.IsValid)
//            {
//                if (LanguageRepository.Update(language))
//                {
//                    return RedirectToAction("Index");
//                }
//                var logHostory = new LogHistory()
//                {
//                    EntityId = language.Id,
//                    EntityType = Core.Enums.EntityTypeCollection.Language,
//                    ActionBy = CurrentUSer,
//                    ActionType = Core.Enums.ActionTypeCollection.Edit,
//                    CreatedDate = DateTime.Now,
//                    Comment = "edit language"
//                };
//                _logHistoryRepository.Insert(logHostory);
//            }
//            return View(language);
//        }

//        public ActionResult Delete(int id)
//        {
//            var model = LanguageRepository.FindId(id);
//            LanguageRepository.Delete(model);
//            var logHostory = new LogHistory()
//            {
//                EntityId = model.Id,
//                EntityType = Core.Enums.EntityTypeCollection.Language,
//                ActionBy = CurrentUSer,
//                ActionType = Core.Enums.ActionTypeCollection.Delete,
//                CreatedDate = DateTime.Now,
//                Comment = "delete language"
//            };
//            _logHistoryRepository.Insert(logHostory);
//            return RedirectToAction("Index");
//        }
//    }
//}
