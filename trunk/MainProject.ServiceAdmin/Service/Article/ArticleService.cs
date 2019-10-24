using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Constant;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.Article;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using MainProject.Web.Areas.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Service.Article
{
    public class ArticleService
    {
        private readonly ArticleRepository _articleRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public ArticleService(MainDbContext dbContext, string currentUser)
        {
            _articleRepository = new ArticleRepository(dbContext);
            _categoryRepository = new CategoryRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Articles);
        }

        /// <summary>
        /// Get data filter for Index View
        /// </summary>
        /// <param name="text">Title of article</param>
        /// <param name="cul">Language of article</param>
        /// <param name="fa">Category of article</param>
        /// <param name="page">Current page</param>
        /// <returns></returns>
        public IndexViewModel<Core.Article> GetIndex(string text, int cul = 0, long fa = 0, int page = 1)
        {
            // Get first language
            if (cul == 0) cul = _languageRepository.FindAll().FirstOrDefault().Id;
            // Check category id is invalid number, it's set by rootId to get all article
            if (fa <= 0) fa = ConfigItemHelper.GetRootNewsId();
            if (page < 1) page = 1;
            var sql =
                _articleRepository.Find(x => x.Language.Id == cul);

            // Check category is not root to filter.
            if (fa != ConfigItemHelper.GetRootNewsId())
            {
                sql = sql.Where(x => x.Category.Id == fa);
            }
            // Filter article by title
            if (!string.IsNullOrEmpty(text))
            {
                sql = sql.Where(x => x.Title.Contains(text));
            }
            // Count total article match filter condition
            var count = sql.Count();
			
            // Bind model for view
            return new IndexViewModel<Core.Article>()
            {
                ListItems = sql.OrderByDescending(x => x.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                FilterViewModel = new FilterViewModel()
                {
                    LanguageViewModel = new LanguageSelectModel()
                    {
                        Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), 0),
                        LanguageSelectedValue = cul
                    },
                    EntityType = EntityTypeCollection.Categories,
                    HasFatherSelect = true,
                    BaseUrl = "/Admin/ArticleAdmin/Index?"
                },
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/ArticleAdmin/Index?cul=" + cul + "&fa=" + fa + "&page={0}'")
            };
        }

        /// <summary>
        /// Prepare data for Create View
        /// </summary>
        /// <returns></returns>
        public ArticleManageViewModel Create()   
        {
            var imageFolder = FolderAndFileHelper.GenerateFolder(FolderConstant.Article);

            return new ArticleManageViewModel
            {
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), 0
                                                        , new SelectListItem() {
                                                            Text = "Vui lòng chọn ngôn ngữ!",
                                                            Value = ""
                                                        }),
                Categories = new List<SelectListItem> { },
                LogHistoryId = _logHistoryService.Create(new LogHistoryModel()
                {
                    ActionType = ActionTypeCollection.Temp,
                    Comment = imageFolder
                }),
                ImageFolder = imageFolder,
            };
        }
        /// <summary>
        /// Insert data to db
        /// </summary>
        /// <param name="model"></param>
        public void Insert(ArticleManageViewModel model)
        {
            // Initialize Entity
            var entity = new Core.Article()
            {
                Category = _categoryRepository.FindId(model.CategorySelectedValue),
                CreateDate = DateTime.Now,
                Language = _languageRepository.FindId(model.LanguageSelectedValue),
                UpdateDate = DateTime.Now,
            };
            // Parse data from model to entity
            ArticleManageViewModel.ToEntity(model, ref entity);
            // Insert to Article Id for generating url
            _articleRepository.Insert(entity);
            // Generating url and SeName
            entity.SeName = SeoHelper.ValidateSeNameAndSubmit(EntityTypeCollection.Articles, entity.Id, model.SeName, entity.Title, entity.ExternalUrl, entity.Language.Id);
            // Update save SeName
            _articleRepository.SaveChanges();
            // Update change logHistory from temp to create
            _logHistoryService.Update(model.LogHistoryId, entity.Id);
        }

        /// <summary>
        /// Get artice for Article Edit view
        /// </summary>
        /// <param name="id">Article id</param>
        /// <returns></returns>
        public BaseResponseModel<ArticleManageViewModel> Edit(int id)
        {
            // Get article for binding data to model
            var article = _articleRepository.FindUnique(x => x.Id == id);
            // Check article is exist
            if (article == null)
                return new BaseResponseModel<ArticleManageViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Bài viết không tồn tại!</strong>")
                };
            // Bind model return to view
            var model = new ArticleManageViewModel(article) {
                LanguageSelectedValue = article.Language.Id,
                CategorySelectedValue = article.Category.Id
            };
            // Bind select languages and categories
            BindSelectListItem(model);

            return new BaseResponseModel<ArticleManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update article into db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel Update(ArticleManageViewModel model)
        {
            // Get article to update data
            var article = _articleRepository.FindUnique(x => x.Id == model.Id);
            // Check article is exist
            if (article == null)
                return new BaseResponseModel() {
                    Code = HttpStatusCodeCollection.BadRequest
                };
            // Bind data from model to article
            ArticleManageViewModel.ToEntity(model, ref article);
            // Bind category and url
            article.Category = _categoryRepository.FindId(model.CategorySelectedValue);
            article.SeName = SeoHelper.ValidateSeNameAndSubmit(EntityTypeCollection.Articles, article.Id, model.SeName, article.Title, article.ExternalUrl, article.Language.Id);
            _articleRepository.SaveChanges();
            // Save log history
            _logHistoryService.Create(new LogHistoryModel() { EntityId = article.Id, ActionType = ActionTypeCollection.Edit });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Đã cập nhật thành công!</strong>")
            };
        }

        /// <summary>
        /// Delete article in db
        /// </summary>
        /// <param name="id">Article id</param>
        /// <returns></returns>
        public BaseResponseModel<int> Delete(long id)
        {
            var article = _articleRepository.FindId(id);
            if (article == null)
                return new BaseResponseModel<int>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Có lỗi xảy ra không thể xóa!</strong>")
                };

            // Get language id before delete
            int languageId = article.Language.Id;
            // Delete article url
            SeoHelper.DeleteUrlRecord(EntityTypeCollection.Articles, article.Id);
            // Delete entity
            _articleRepository.Delete(article);

            // Save log History
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = article.Id, ActionType = ActionTypeCollection.Delete });

            // Delete folder
            FolderAndFileHelper.DeleteFolder(article.ImageFolder);
            return new BaseResponseModel<int> {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Đã xóa thành công!</strong>"),
                Result = languageId
            };
        }
        /// <summary>
        /// Bind data to drop down in html
        /// </summary>
        /// <param name="model"></param>
        public void BindSelectListItem(ArticleManageViewModel model)
        {
            model.Categories = CategoryHelper.BindSelectListItem(_categoryRepository, model.CategorySelectedValue, model.LanguageSelectedValue);
            model.Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), model.LanguageSelectedValue);
        }
    }
}
