using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.FaqItem;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using System;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.FaqItem
{
    public class FaqItemService
    {
        private readonly FaqItemRepository _faqItemRepository;
        private readonly LogHistoryService _logHistoryService;
        private int _itemPerPage = 15;

        public FaqItemService(MainDbContext dbContext, string currentUser)
        {
            _faqItemRepository = new FaqItemRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.FaqItem);
        }

        public FaqItemsViewModel GetIndex(int status, string title = null, int page = 1)
        {
            // Get all entity
            var sql = _faqItemRepository.FindAll();
            // Filter entity by question content
            if (!string.IsNullOrWhiteSpace(title))
            {
                sql = sql.Where(d => d.AskTitle.Contains(title.Trim()));
            }
            // Filter entity by status
            if (status != (int)FaqApprovalStatusCollection.NotPending)
            {
                sql = sql.Where(d => d.ApprovalStatus == (FaqApprovalStatusCollection)status);
            }

            return new FaqItemsViewModel
            {
                PagingViewModel = new PagingModel(sql.Count(), _itemPerPage, page,
                                                    "href='/Admin/FaqItemAdmin/Index?status=" + status + "&page={0}'"),
                ListFaqItem = sql.OrderByDescending(d => d.AnswerTime).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                StatusSelectedValue = status,
                ApprovalStatusList = EnumHelper.ToSelectList(typeof(FaqApprovalStatusCollection)),
                Title = title ?? ""
            };
        }

        /// <summary>
        /// Prepare data for Detail View
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        //public Core.FaqItem GetDetail(long id, string currentUser)
        //{
        //    var model = _faqItemRepository.FindUnique(d => d.Id == id);
        //    model.AnswerName = currentUser;
        //    return model;
        //}

        /// <summary>
        /// Prepare data for Answer View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<FaqItemAnswerManageModel> Answer(long id)
        {
            var entity = _faqItemRepository.FindUnique(d => d.Id == id);
            if (entity != null)
            {
                return new BaseResponseModel<FaqItemAnswerManageModel> {
                    Code = HttpStatusCodeCollection.OK,
                    Result = new FaqItemAnswerManageModel(entity)
                };
            }
            return new BaseResponseModel<FaqItemAnswerManageModel> {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = string.Format("<strong style='color:red'>Không tìm thấy câu hỏi</strong>")
            };
        }

        public void Answer(FaqItemAnswerManageModel faqItemAnswerManageModel, string CurrentUser)
        {
            var model = _faqItemRepository.FindUnique(x => x.Id == faqItemAnswerManageModel.Id);
            model.AnswerTime = DateTime.Now;
            model.AnswerContent = faqItemAnswerManageModel.AnswerContent;
            model.ApprovalStatus = faqItemAnswerManageModel.IsPending 
                                        ? FaqApprovalStatusCollection.Accept : FaqApprovalStatusCollection.NotPending;
            model.AnswerName = CurrentUser;
            _faqItemRepository.SaveChanges();
            _logHistoryService.Create(new LogHistoryModel
            {
                ActionType = ActionTypeCollection.Answer,
                EntityId = model.Id,
                Comment = "Answer faq item"
            });    
        }
        //public FaqItemAnswerManageModel Eidt(long id)
        //{
        //    var model = _faqItemRepository.FindUnique(d => d.Id == id);
        //    if (model != null)
        //    {
        //        var faqitem = new FaqItemAnswerManageModel(model);
        //        return faqitem;
        //    }
        //    return null;
        //}

        //public void Eidt(FaqItemAnswerManageModel faqItemAnswerManageModel, string CurrentUser )
        //{
        //    var model = _faqItemRepository.FindUnique(x => x.Id == faqItemAnswerManageModel.Id);
        //    model.AnswerTime = DateTime.Now;
        //    model.AnswerContent = faqItemAnswerManageModel.AnswerContent;
        //    if (faqItemAnswerManageModel.IsPending)
        //    {
        //        model.ApprovalStatus = FaqApprovalStatusCollection.Pending;
        //    }
        //    else
        //    {
        //        model.ApprovalStatus = FaqApprovalStatusCollection.NotPending;
        //    }
        //    model.AnswerName = CurrentUser;
        //    _faqItemRepository.SaveChanges();
        //    _logHistoryService.Create(new LogHistoryModel
        //    {
        //        ActionType = ActionTypeCollection.Answer,
        //        EntityId = model.Id,
        //        Comment = "Eidt faq item"
        //    });  
        //}

        //public MessageViewModel Approve(long id,string CurrentUser)
        //{
        //    var res = new MessageViewModel()
        //    {
        //        Result = true,
        //        Message = "Không tìm thấy dữ liệu phù hợp.",
        //        CallBack = string.Empty
        //    };
        //    var model = _faqItemRepository.FindUnique(d => d.Id == id);
        //    if (model != null)
        //    {
        //        if (model.ApprovalStatus == FaqApprovalStatusCollection.NotPending)
        //        {
        //            res.Message = "Không được xử lý. FAQ này không được tác giả đánh dấu chờ approve.";
        //        }
        //        else
        //        {
        //            res.Result = true;
        //            if (model.ApprovalStatus == FaqApprovalStatusCollection.Accept)
        //            {
        //               res.Message =
        //                    "FAQ này đã được approve bởi một người nào trước đó. Xin vui lòng xem log để biết thêm thông tin và refresh lại page để cập nhật dữ liệu mới. ";
        //            }
        //            else
        //            {
        //                model.ApprovalStatus = FaqApprovalStatusCollection.Accept;
        //                _faqItemRepository.SaveChanges();
        //                model.AnswerName = CurrentUser;
        //                res.Message = "FAQ này đã được cho phép hiển thị.";
        //            }
        //            res.CallBack = "/Admin/FaqItemAdmin/Index?status=" + model.ApprovalStatus + "&page={0}";
        //        }
        //        return res;
        //    }
        //    res.Result = false;
        //    return res;
        //}

        //public MessageViewModel Reject(long id,string CurrentUser)
        //{
        //    var res = new MessageViewModel()
        //    {
        //        Result = true,
        //        Message = "Không tìm thấy dữ liệu phù hợp.",
        //        CallBack = string.Empty
        //    };

        //    var model = _faqItemRepository.FindUnique(d => d.Id == id);
        //    if (model != null)
        //    {
        //        if (model.ApprovalStatus == FaqApprovalStatusCollection.NotPending)
        //        {
        //            res.Message = "Không được xử lý. FAQ này không được tác giả đánh dấu chờ approve.";
        //        }
        //        else
        //        {
        //            res.Result = true;
        //            if (model.ApprovalStatus == FaqApprovalStatusCollection.Accept)
        //            {
        //                res.Message =
        //                    "FAQ này đã được approve bởi một người nào trước đó. Xin vui lòng xem log để biết thêm thông tin và refresh lại page để cập nhật dữ liệu mới. ";
        //            }
        //            else if (model.ApprovalStatus == FaqApprovalStatusCollection.Reject)
        //            {
        //                res.Message =
        //                   "FAQ này đã được reject bởi một người nào trước đó. Xin vui lòng xem log để biết thêm thông tin và refresh lại page để cập nhật dữ liệu mới. ";
        //            }
        //            else
        //            {
        //                model.ApprovalStatus = FaqApprovalStatusCollection.Reject;
        //                _faqItemRepository.SaveChanges();
        //                model.AnswerName = CurrentUser;
        //                res.Message = "FAQ này đã không được cho phép hiển thị.";
        //            }
        //            res.CallBack = "/Admin/FaqItemAdmin/Index?status=" + model.ApprovalStatus + "&page={0}";
        //        }
        //        res.Result = false;
        //        return res;
        //    }
        //    res.Result = false;
        //    return res;

        //}
        //public long Next(long id)
        //{
        //    var model = _faqItemRepository.FindUnique(d => d.Id == id);
        //    return  _faqItemRepository.FindAll().Where(d => d.Id > id).ToList().First().Id;
            
        //}

        public string Delete(long id)
        {
            // Get entity to delete
            var entity = _faqItemRepository.FindUnique(d => d.Id == id);
            if (entity != null)
            {
                // Delete entity
                _faqItemRepository.Delete(entity);
                _faqItemRepository.SaveChanges();
                // Save log history
                _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });       
               return string.Format("<strong style='color:green'>Đã xóa thành công</strong>");
            }
            else
            {
                return string.Format("<strong style='color:red'>Không tìm thấy đối tượng cần xóa</strong>");
            }
        }
    }
}
