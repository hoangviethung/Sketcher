using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;
using MainProject.Data;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model.Banner;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.Framework.Constant;

namespace MainProject.ServiceAdmin.Service.Banner
{
    public class BannerService
    {
        private readonly BannerRepository _bannerRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 15;

        public BannerService(MainDbContext dbContext, string currentUser)
        {
            _bannerRepository = new BannerRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Banner);
        }

        /// <summary>
        /// Filter banners
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IndexViewModel<MainProject.Core.Banner> GetIndex(int languageId = 0, int page = 1)
        {
            languageId = languageId != 0 ? languageId : _languageRepository.FindAll().First().Id;
            if (page < 1) page = 1;
            // Get banners by language
            var sql = _bannerRepository.Find(x => x.Language.Id == languageId);
            // Count total banner
            int count = sql.Count();
            var banners = sql.OrderBy(d => d.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList();

            return new IndexViewModel<MainProject.Core.Banner>()
            {
                ListItems = banners,
                FilterViewModel = new FilterViewModel()
                {
                    LanguageViewModel = new LanguageSelectModel()
                    {
                        Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), languageId),
                        LanguageSelectedValue = languageId
                    },
                    BaseUrl = "/Admin/BannerAdmin/Index?"
                },
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/BannerAdmin/Index?languageId=" + languageId + "&page={0}'")
            };
        }

        /// <summary>
        /// Initialize data for create view
        /// </summary>
        /// <returns></returns> 
        public BannerManageViewModel Create()
        {
            var imageFolder = FolderAndFileHelper.GenerateFolder(FolderConstant.Banner);

            return new BannerManageViewModel
            {
                ImageFolder = imageFolder,
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), 0),
                LogHistoryId = _logHistoryService.Create(new LogHistoryModel()
                {
                    ActionType = ActionTypeCollection.Temp,
                    Comment = imageFolder
                }),
            };
        }

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <param name="model"></param>
        public void Insert(BannerManageViewModel model)
        {
            var banner = new Core.Banner
            {
                Language = _languageRepository.FindId(model.LanguageSelectedValue)
            };
            // Parse data from model to entity
            model.ToEntity(model, ref banner);
            // Insert to banner Id for generating url
            _bannerRepository.Insert(banner);
			// Update change logHistory from temp to create
            _logHistoryService.Update(model.LogHistoryId ,banner.Id);
        }

        /// <summary>
        /// Get banner for Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<BannerManageViewModel> Edit(long id)
        {
            // Get entity bind data to model
            var entity = _bannerRepository.FindUnique(x => x.Id == id);
            // Check entity is exist
            if (entity == null)
            {
                return new BaseResponseModel<BannerManageViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Không tìm thấy banner cần sửa</strong>")
                };
            }

            // Bind data to model
            var model = new BannerManageViewModel(entity);
            // Bind language select list
            BindSelectListItem(model);

            return new BaseResponseModel<BannerManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update data banner to databse
        /// </summary>
        /// <param name="model"></param>
        public BaseResponseModel Update(BannerManageViewModel model)
        {
            // Get entity to update data
            var entity = _bannerRepository.FindUnique(d => d.Id == model.Id);
            if (entity == null)
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Banner không tồn tại!</strong>")
                };
            // Parse data from model to entity
            model.ToEntity(model, ref entity);
            // Save changes data banner to entity
            _bannerRepository.SaveChanges();
            // save logHistory 
            _logHistoryService.Create(new LogHistoryModel { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Đã cập nhật banner thành công!</strong>")
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
            var entity = _bannerRepository.FindById(id);
            // Check banner is exist
            if (entity == null)
                return string.Format("Có lỗi xảy ra không thể xóa được!");
            // Delete banner
            _bannerRepository.Delete(entity);
            // Save history
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });
            // Delete image folder
            FolderAndFileHelper.DeleteFolder(entity.ImageFolder);

            return string.Format("<strong style='color:green'>Xóa banner thành công!</strong>");
        }

        /// <summary>
        /// Bind select listItem
        /// </summary>
        /// <param name="model"></param>
        public void BindSelectListItem(BannerManageViewModel model)
        {
            model.Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), model.LanguageSelectedValue);
        }
    }
}
