using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.CommerceProperty;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.CommerceProperty
{
    public class CommercePropertyService
    {
        private readonly LogHistoryService _logHistoryService;
        private readonly PropertyRepository _propertyRepository;
        private readonly ProductPropertyRefRepository _productPropertyRefRepository;
        private readonly int _itemPerPage = 10;

        public CommercePropertyService(MainDbContext dbContext)
        {
            _propertyRepository = new PropertyRepository(dbContext);
            _productPropertyRefRepository = new ProductPropertyRefRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.CommerceProperty);
        }

        public IndexViewModel<Core.Commerce.Property> GetIndex(int page)
        {
            if (page < 1) page = 1;
            var query = _propertyRepository.FindAll();

            return new IndexViewModel<Core.Commerce.Property>
            {
                ListItems = query.OrderBy(c => c.Name).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                PagingViewModel = new PagingModel(query.Count(), _itemPerPage, page, "href='/Admin/CommercePropertyAdmin/Index?page={0}'"),
            };
        }

        /// <summary>
        /// Prepare data for Create View
        /// </summary>
        /// <returns></returns>
        public PropertyManageModel Create()
            => new PropertyManageModel(null);

        /// <summary>
        /// Prepare data for Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<PropertyManageModel> Edit(long id)
        {
            // Get entity to bind model
            var entity = _propertyRepository.FindUnique(c => c.Id == id);
            // Check entity is exist
            if (entity == null)
                return new BaseResponseModel<PropertyManageModel>
                {
                    Code = HttpStatusCodeCollection.NotFound,
                    Message = string.Format("<strong style='color:green'>Đối tượng không tồn tại!</strong>")
                };

            return new BaseResponseModel<PropertyManageModel>
            {
                Code = HttpStatusCodeCollection.OK,
                // Bind entity to model
                Result = new PropertyManageModel(entity)
            };          
        }

        public BaseResponseModel Manage(PropertyManageModel model)
            => CommercePropertyHelper.CreateOrUpdate(_propertyRepository, model, _logHistoryService);

        public string Delete(long id)
        {
            // Get entity to delete
            var entity = _propertyRepository.FindUnique(c => c.Id == id);
            // Check entity is exist
            if(entity == null)
                return string.Format("<strong style='color:red'>Xảy ra lỗi trong quá trình xóa. Xin vui lòng thử lại!</strong>");
            // Delete property reference
            _productPropertyRefRepository.DeleteByCriteria(c => c.Property.Id == entity.Id);
            // Delete property
            _propertyRepository.Delete(entity);
            // Save log History
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });

            return string.Format("<strong style='color:green'>Đã xóa thành công!</strong>");
        }


        public void ReInitManageModel(PropertyManageModel model)
        {
            model.UnitTypeItems = EnumHelper.ToSelectList(typeof(UnitTypeCollection), model.UnitTypeSelected);
        }
    }
}
