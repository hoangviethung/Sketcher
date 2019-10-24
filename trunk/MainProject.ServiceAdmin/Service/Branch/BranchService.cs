using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.Branch;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Branch
{
    public class BranchService
    {
        private readonly BranchRepository _branchRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public BranchService(MainDbContext dbContext, string currentUser)
        {
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Branch);
            _branchRepository = new BranchRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
        }

        /// <summary>
        /// Filter branch
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IndexViewModel<MainProject.Core.Branch> GetIndex(int languageId = 0, int page = 1)
        {
            languageId = languageId != 0 ? languageId : _languageRepository.FindAll().First().Id;
            if (page < 1) page = 1;
            // Get banners by language
            var sql = _branchRepository.Find(x => x.Language.Id == languageId);
            // Count total banner
            int count = sql.Count();
            var branches = sql.OrderBy(d => d.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList();

            return new IndexViewModel<MainProject.Core.Branch>()
            {
                ListItems = branches,
                FilterViewModel = new FilterViewModel()
                {
                    LanguageViewModel = new LanguageSelectModel()
                    {
                        Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), languageId),
                        LanguageSelectedValue = languageId
                    },
                    BaseUrl = "/Admin/BranchAdmin/Index?"
                },
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/BranchAdmin/Index?languageId=" + languageId + "&page={0}'")
            };
        }

        /// <summary>
        /// Initialize data for create view
        /// </summary>
        /// <returns></returns> 
        public BranchManageViewModel Create()
        {
            return new BranchManageViewModel
            {
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), 0),
            };
        }

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <param name="model"></param>
        public void Insert(BranchManageViewModel model)
        {
            var entity = new Core.Branch
            {
                Language = _languageRepository.FindId(model.LanguageSelectedValue)
            };
            // Parse data from model to entity
            model.ToEntity(ref entity, model);
            // Insert to banner Id for generating url
            _branchRepository.Insert(entity);
            // Create logHistory
            _logHistoryService.Create(new LogHistoryModel() { ActionType = ActionTypeCollection.Create, EntityId = entity.Id });
        }

        /// <summary>
        /// Get branch for Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<BranchManageViewModel> Edit(long id)
        {
            // Get entity bind data to model
            var entity = _branchRepository.FindUnique(x => x.Id == id);
            // Check entity is exist
            if (entity == null)
            {
                return new BaseResponseModel<BranchManageViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Không tìm thấy đối tượng</strong>")
                };
            }

            // Bind data to model
            var model = new BranchManageViewModel(entity);
            // Bind language select list
            BindSelectListItem(model);

            return new BaseResponseModel<BranchManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update data branch to databse
        /// </summary>
        /// <param name="model"></param>
        public BaseResponseModel Update(BranchManageViewModel model)
        {
            // Get entity to update data
            var entity = _branchRepository.FindUnique(d => d.Id == model.Id);
            if (entity == null)
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Chi nhánh không tồn tại!</strong>")
                };
            // Parse data from model to entity
            model.ToEntity(ref entity, model);
			entity.Language = _languageRepository.FindId(model.LanguageSelectedValue);
            // Save changes data banner to entity
            _branchRepository.SaveChanges();
            // save logHistory 
            _logHistoryService.Create(new LogHistoryModel { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Đã cập nhật chi nhánh thành công!</strong>")
            };
        }

        /// <summary>
        /// Delete data banner form database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // Get entity to delete
            var entity = _branchRepository.FindUnique(x=>x.Id == id);
            // Check banner is exist
            if (entity == null)
                return string.Format("Có lỗi xảy ra không thể xóa được!");
            // Delete banner
            _branchRepository.Delete(entity);
            // Save history
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });

            return string.Format("<strong style='color:green'>Xóa chi nhánh thành công!</strong>");
        }

        /// <summary>
        /// Bind select listItem
        /// </summary>
        /// <param name="model"></param>
        public void BindSelectListItem(BranchManageViewModel model)
        {
            model.Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), model.LanguageSelectedValue);
        }

    }
}
