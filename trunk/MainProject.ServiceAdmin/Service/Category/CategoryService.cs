using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model.Category;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using MainProject.Web.Areas.Admin.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using MainProject.Framework.Constant;

namespace MainProject.ServiceAdmin.Service.Category
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public CategoryService(MainDbContext dbContext, string currentUser)
        {
            _categoryRepository = new CategoryRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Categories);
        }

        /// <summary>
        /// Filter categories
        /// </summary>
        /// <param name="languageId">Language of category</param>
        /// <param name="page">Current page</param>
        /// <returns></returns>
        public IndexViewModel<Core.Category> GetIndex(int languageId = 0, int page = 1)
        {
            if (languageId == 0) languageId = _languageRepository.FindAll().FirstOrDefault().Id;
            if (page < 1) page = 1;
            var sql =
                _categoryRepository.Find(x => !x.IsSystem && x.Language.Id == languageId);

            var count = sql.Count();
            var categories = sql.OrderByDescending(x => x.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList();

            return new IndexViewModel<Core.Category>()
            {
                ListItems = categories,
                FilterViewModel = new FilterViewModel()
                {
                    LanguageViewModel = new LanguageSelectModel()
                    {
                        Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), languageId),
                        LanguageSelectedValue = languageId
                    },
                    EntityType = EntityTypeCollection.Categories,
                    HasFatherSelect = true,
                    BaseUrl = "/Admin/CategoryAdmin/Index?"
                },
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/CategoryAdmin/Index?languageId=" + languageId + "&page={0}'")
            };
        }

        /// <summary>
        /// Initialize data for Create View
        /// </summary>
        /// <returns></returns>
        public CategoryManageViewModel Create()
        {
            var imageFolder = FolderAndFileHelper.GenerateFolder(FolderConstant.Category);

            return new CategoryManageViewModel
            {
                ImageFolder = imageFolder,
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), 0),
                Parents = new List<SelectListItem>
                                              {
                                                  new SelectListItem
                                                      {
                                                          Value = "",
                                                          Text = "Vui lòng chọn danh mục!"
                                                      }
                                              },
                DisplayTemplates = EnumHelper.ToSelectList(typeof(DisplayTemplateCollection)),
                // Save logHistory in case User cancel create, this use for deleting unused folder
                LogHistoryId = _logHistoryService.Create(new LogHistoryModel(){
                    ActionType = ActionTypeCollection.Temp,
                    Comment = imageFolder,
                })
            };
        }

        /// <summary>
        /// Insert data into db
        /// </summary>
        /// <param name="model"></param>
        public void Insert(CategoryManageViewModel model)
        {
            // Initialize Entity
            var entity = new Core.Category
            {
                Language = _languageRepository.FindId(model.LanguageSelectedValue),
                Parent = _categoryRepository.FindId(model.ParentSelectedValue),
                PrivateArea = model.PrivateArea,
                EntityType = EntityTypeCollection.Categories,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };
            // Parse data from model to entity
            CategoryManageViewModel.ToEntity(model, ref entity);
            // Insert to category Id for generating url
            _categoryRepository.Insert(entity);
            // Generating url and SeName
            entity.SeName = SeoHelper.ValidateSeNameAndSubmit(EntityTypeCollection.Categories, entity.Id, entity.SeName, entity.Title, entity.ExternalUrl, entity.Language.Id);
            // Update save SeName
            _categoryRepository.SaveChanges();
            // Update change logHistory from temp to create
            _logHistoryService.Update(model.LogHistoryId, entity.Id);
        }

        /// <summary>
        /// Get category for Category Edit View
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public BaseResponseModel<CategoryManageViewModel> Edit(long id)
        {
            var entity = _categoryRepository.FindId(id);
            if (entity == null)
            {
                return new BaseResponseModel<CategoryManageViewModel>{
                    Code = HttpStatusCodeCollection.BadRequest,
                };
            }

            var model = new CategoryManageViewModel(entity);
            BindSelectListItem(model);

            return new BaseResponseModel<CategoryManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update category into db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(CategoryManageViewModel model)
        {
            // Get edited category for changing data
            var entity = _categoryRepository.FindId(model.Id);
            if (entity == null)
                return false;
            // Bind data want to edit from model to entity
            CategoryManageViewModel.ToEntity(model, ref entity);
            entity.UpdateDate = DateTime.Now;
            // Get parent category
            entity.Parent = _categoryRepository.FindId(model.ParentSelectedValue);
            _categoryRepository.SaveChanges();
            entity.SeName = SeoHelper.ValidateSeNameAndSubmit(EntityTypeCollection.Categories, entity.Id, model.SeName, entity.Title, entity.ExternalUrl, entity.Language.Id);
            _categoryRepository.SaveChanges();
            // Create log history for editting category
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });
            return true;
        }

        /// <summary>
        /// Delete article in db
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // Get category for checking exist and deleting
            var entity = _categoryRepository.FindId(id);
            if (entity == null)
                return string.Format("<strong style='color:red'>Có lỗi xảy ra không thể xóa!</strong>");

            // Check category has any parent
            if (_categoryRepository.HasParent(id))
                return string.Format("<strong style='color:red'>{0} có nhóm con, không thể xóa</strong>", entity.Title);

            if (_categoryRepository.HasArticle(id))
                return string.Format("<strong style='color:red'>{0} có bài viết con, không thể xóa</strong>", entity.Title);

            // Delete Url
            SeoHelper.DeleteUrlRecord(EntityTypeCollection.Categories, entity.Id);
            // Delete category
            _categoryRepository.Delete(entity);

            // Save log History
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });

            // Delete folder
            FolderAndFileHelper.DeleteFolder(entity.ImageFolder);

            return string.Format("<strong style='color:green'>Xóa danh mục thành công!</strong>");
        }

        /// <summary>
        /// Bind data to drop down list in html
        /// </summary>
        /// <param name="model"></param>
        public void BindSelectListItem(CategoryManageViewModel model)
        {
            model.DisplayTemplates = EnumHelper.ToSelectList(typeof(DisplayTemplateCollection));
            model.Parents = CategoryHelper.BindSelectListItem(_categoryRepository, model.ParentSelectedValue, model.LanguageSelectedValue, model.Id,true);
            model.Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), model.LanguageSelectedValue);
        }
    }
}
