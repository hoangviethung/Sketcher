using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Subscribe;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Subscribe
{
    public class SubscribeService
    {
        private readonly SubscribeRepository _subscribeRepository;
        private readonly LogHistoryService _logHistoryService;

        private readonly int pageitem = 10;

        public SubscribeService(MainDbContext dbContext, string currentUser)
        {
            _subscribeRepository = new SubscribeRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Subscribe);
        }

        /// <summary>
        /// Get index 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public IndexViewModel<Core.Subscribe> GetIndex(int page = 1)
        {
            var sql = _subscribeRepository.FindAll();
            var count = sql.Count();
            var subscribes = sql.OrderByDescending(p => p.Id).Skip((page - 1) * pageitem).Take(pageitem).ToList();
            return new IndexViewModel<Core.Subscribe>()
            {
                ListItems = subscribes,
                PagingViewModel = new PagingModel(count, pageitem, page, "href='/Admin/SubscribeAdmin/Index?page={0}'")

            };
        }

        /// <summary>
        /// delete subsribe
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // Get subscribe for checking exist and deleting
            var entity = _subscribeRepository.FindUnique(x => x.Id == id);
            if (entity == null)
                return string.Format("Có lỗi xảy ra không thể xóa được!");
            // Delete subscribe
            _subscribeRepository.Delete(entity);
            // Save history
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });

            return string.Format("<strong style='color:green'>Xóa subsribe thành công</strong>");
        }

        /// <summary>
        /// Get id for edit note
        /// </summary>
        /// <param name="note"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel EditNote(bool modelState, SubscribeManageModel model)
        {
            if (modelState)
            {
                var entity = _subscribeRepository.FindUnique(x => x.Id == model.Id);
                if (entity == null)
                {
                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.BadRequest,
                        Message = string.Format("<strong style='color:red'>Email này không tồn tại!</strong>")
                    };
                }

                entity.Note = model.Note;
                _subscribeRepository.SaveChanges();
                // save logHistory 
                _logHistoryService.Create(new LogHistoryModel { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

                return new BaseResponseModel
                {
                    Code = HttpStatusCodeCollection.OK,
                    Message = string.Format("<strong style='color:green'>Cập nhật lưu ý Email thành công!</strong>")
                };
            }

            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = string.Format("<strong style='color:red'>Ghi chú không được vượt quá 500 ký tự!</strong>")
            };
        }

    }
}
