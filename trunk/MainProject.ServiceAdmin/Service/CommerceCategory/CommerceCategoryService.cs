using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.CommerceCategory;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Service.CommerceCategory
{
    public class CommerceCategoryService
    {
        private readonly LanguageRepository _languageRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly CommerceCategoryRepository _commerceCategoryRepository;
        private readonly ProductCommerceCategoryRefRepository _productCommerceCategoryRefRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public CommerceCategoryService(MainDbContext dbContext)
        {
            _categoryRepository = new CategoryRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _commerceCategoryRepository = new CommerceCategoryRepository(dbContext);
            _productCommerceCategoryRefRepository = new ProductCommerceCategoryRefRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.CommerceCategory);
        }

        /// <summary>
        /// Get data for Index View
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public IndexViewModel<CommerceCategoryModel> GetIndex(int page = 1)
        {
            if (page < 1) page = 1;
            var query = _commerceCategoryRepository.FindAll();

            return new IndexViewModel<CommerceCategoryModel>
            {
                PagingViewModel = new PagingModel(query.Count(), _itemPerPage, page, "href='/Admin/CommerceCategoryAdmin/Index?page={0}'"),
                ListItems = query.OrderBy(c => c.Parent.Order).ThenBy(c => c.Order).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList()
                                    .Select(c => new CommerceCategoryModel(c)).ToList()
            };
        }

        /// <summary>
        /// Prepare data for Create View
        /// </summary>
        /// <returns></returns>
        public CommerceCategoryManageModel Create()
        {
            // Bind data to model
            var model = CommerceCategoryManageModel.InitManageModel(null);
            // Save log History
            model.LogHistoryId = _logHistoryService.Create(new LogHistoryModel() { ActionType = ActionTypeCollection.Temp });

            return model;
        }

        /// <summary>
        /// Prepare data for Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<CommerceCategoryManageModel> Edit(long id)
        {
            var entity = _commerceCategoryRepository.FindUnique(c => c.Id == id);
            if (entity == null)
            {
                return new BaseResponseModel<CommerceCategoryManageModel>
                {
                    Code = HttpStatusCodeCollection.NotFound,
                    Message = string.Format("<strong style='color:green'>Đối tượng không tồn tại!</strong>")
                };
            }

            return new BaseResponseModel<CommerceCategoryManageModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = CommerceCategoryManageModel.InitManageModel(entity)
            };
        }


        /// <summary>
        /// Update or Insert data into db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel Manage(CommerceCategoryManageModel model)
            => CommerceCategoryHelper.CreateOrUpdate(
                        _commerceCategoryRepository, model, _logHistoryService, _categoryRepository);

        /// <summary>
        /// Delete data into db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // Get entity to delete
            var entity = _commerceCategoryRepository.FindUnique(c => c.Id == id);
            // Check entity is exist
            if (entity != null)
            {
                if (_commerceCategoryRepository.ExistChild(id))
                {
                    return string.Format("<strong style='color:red'>Danh mục sản phẩm này có nhóm con không thể xóa!</strong>");
                }
                // Delete reference commerce category to product
                _productCommerceCategoryRefRepository.DeleteByCriteria(c => c.CommerceCategory.Id == id);
                // Delete commerce category
                _commerceCategoryRepository.Delete(entity);
                _commerceCategoryRepository.SaveChanges();
                // Save log History
                _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });

                return string.Format("<strong style='color:green'>Đã xóa thành công!</strong>");
            }

            return string.Format("<strong style='color:red'>Xảy ra lỗi trong quá trình xóa. Xin vui lòng thử lại!</strong>");
        }

        public void ReInitManageModel(CommerceCategoryManageModel model)
        {
            model.Parents = CommerceCategoryHelper.BindSelectListItem(
                                    _commerceCategoryRepository, model.ParentSelectedValue, model.Id == 0 ? -1 : model.Id, true);
        }

    }
}
