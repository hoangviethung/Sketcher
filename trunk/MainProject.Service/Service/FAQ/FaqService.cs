//using MainProject.Core;
//using MainProject.Core.Enums;
//using MainProject.Data;
//using MainProject.Data.Repositories;
//using MainProject.Framework.Helper;
//using MainProject.Service.Model.FAQ;
//using System;
//using System.Linq;

//namespace MainProject.Service.FAQ
//{
//    public class FaqService
//    {
//        private readonly FaqItemRepository _faqItemRepository;
//        private readonly CategoryRepository _categoryRepository;
//        private const int FaqPageItems = 3;

//        public FaqService(MainDbContext dbContext)
//        {
//            _faqItemRepository = new FaqItemRepository(dbContext);
//            _categoryRepository = new CategoryRepository(dbContext);
//        }

//        public FaqItemsViewModel Get(int languageId = 1, int page = 1)
//        {
//            var category = _categoryRepository.FindUnique(x => x.Language.Id == languageId
//                                                              && x.DisplayTemplate == DisplayTemplateCollection.FaqTemplate);
//            if (category == null) return null;

//            var sql = _faqItemRepository.Find( d => d.ApprovalStatus == FaqApprovalStatusCollection.Accept).ToList();
            
//            return new FaqItemsViewModel()
//            {
//                ListFaqItem = sql.OrderBy(d => d.AskTime).Skip((page - 1) * FaqPageItems).Take(FaqPageItems).ToList(),
//                Category = category ,
//            };
//        }

//        public void Load(ref string str, ref bool isCanLoadMore, int page = 1, bool isMobile = false)
//        {
//            if (page < 1) page = 1;
//            var sql = _faqItemRepository.FindAll().Where(
//                d =>
//                    d.ApprovalStatus == FaqApprovalStatusCollection.Accept).ToList();
//            var ListFaqItem = sql.OrderBy(d => d.AskTime).Skip((page - 1) * FaqPageItems).Take(FaqPageItems).ToList();
//            isCanLoadMore = sql.Count() > (page * FaqPageItems);
//            var stt = (page - 1) * FaqPageItems;
//            if (!isMobile)
//            {
//                foreach (var item in ListFaqItem)
//                {
//                    stt++;
//                    var sttStr = (stt) < 10 ? "0" + stt.ToString() : stt.ToString();
//                    str += "<div class=\"item-answer\">"
//                           + "<div class=\"row no_mg phase\">"
//                               + "<div class=\"stt\">"
//                                 + "<h3>" + sttStr + "</h3>"
//                              + "</div>"
//                               + "<div class=\"content-answer\">"
//                                    + "<div class=\"question\">"
//                                 + "<p>" + item.AskContent + "</p>"
//                                 + "</div>"
//                                  + "<div class=\"answer\">"
//                                   + "<p>" + item.AnswerContent + "</p>"
//                                 + "</div>"
//                              + "</div>"
//                         + "</div>"
//                       + "</div>";
//                }
//            }
//            else
//            {
//                foreach (var item in ListFaqItem)
//                {
//                    str += "<div class=\"item_comment\">"
//                            + "<div class=\"ct_item_cm\">"
//                               + "<h2>" + item.AskContent + "</h2>"
//                                + "<p>" + item.AnswerContent + "</p>"
//                           + "</div>"
//                       + "</div>";
//                }
//            }
//        }

//        public string Save(FaqQuestionViewModel model)
//        {
//            var entity = new FaqItem()
//            {
//                AskTime = DateTime.Now,
//                AnswerTime = DateTime.Now,
//            };

//            FaqQuestionViewModel.ToEntity(model, ref entity);
//            _faqItemRepository.Insert(entity);
//            return string.Format(ResourceHelper.GetResource(ResourceKeyCollection.AddContactSuccess));
//        }
//    }
//}
