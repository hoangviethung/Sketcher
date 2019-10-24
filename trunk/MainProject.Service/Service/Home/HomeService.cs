using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.Service.Helper;
using MainProject.Service.Model;
using MainProject.Service.Model.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MainProject.Service.Service.Home
{
    public class HomeService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ArticleRepository _articleRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly MenuRepository _menuRepository;
        private readonly MenuItemRepository _menuItemRepository;
        private readonly SubscribeRepository _subscribeRepository;

        public HomeService(MainDbContext dbContext)
        {
            _categoryRepository = new CategoryRepository(dbContext);
            _articleRepository = new ArticleRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _menuRepository = new MenuRepository(dbContext);
            _menuItemRepository = new MenuItemRepository(dbContext);
            _subscribeRepository = new SubscribeRepository(dbContext);
        }

        public HomeViewModel Get(int languageId = 1)
        {
            var category = _categoryRepository.FindUnique(
                                    x => x.DisplayTemplate == DisplayTemplateCollection.Home && x.Language.Id == languageId);
            if (category == null)
                return null;

            // Get data here

            return new HomeViewModel()
            {
                Category = category
            };
        }

        public List<string> GetImages(string folder)
        {
            // Check folder is Upload
            if (folder.IndexOf("/Upload/") != 0)
                return null;

            var images = new List<string>();
            var directorInfo = new DirectoryInfo(FolderAndFileHelper.GetMapPath(folder));
            if (directorInfo.Exists)
            {
                foreach (var fileInfo in directorInfo.GetFiles())
                {
                    images.Add(folder + "/" + fileInfo.Name);
                }
            }

            return images;
        }

        public Category GetCategory(long id)
        {
            return _categoryRepository.FindId(id);
        }

        public Core.Article GetArticle(long id)
        {
            var article = _articleRepository.FindId(id);
            article.ViewCount++;
            _articleRepository.SaveChanges();

            return article;
        }

        public HeaderViewModel GetHeader(int languageId = 1, bool isMobile = false)
        {
            var mainmnu_html = MenuHelper.GetMenu(
                    _menuRepository.FindUnique(x => x.CodeName == "main-menu" && x.Type == MenuTypeCollection.MainMenu),
                        _languageRepository.FindUnique(x => x.Id == languageId), _menuItemRepository, isMobile);


            return new HeaderViewModel()
            {
                MainMenu = mainmnu_html,
            };
        }
		
		public FooterViewModel GetFooter(int languageId = 1, bool isMobile = false)
        {
            var mainmnu_html = MenuHelper.GetFooterMenu(
                    _menuRepository.FindUnique(x => x.CodeName == "footer-menu" && x.Type == MenuTypeCollection.FooterMenu),
                        _languageRepository.FindUnique(x => x.Id == languageId), _menuItemRepository, isMobile);

            return new FooterViewModel()
            {
                MainMenu = mainmnu_html,
                //Facebook = SettingHelper.GetValueSetting("FacebookPage"),
                //Youtube = SettingHelper.GetValueSetting("YoutubePage"),
                //Twitter = SettingHelper.GetValueSetting("TwitterPage"),
                //Instagram = SettingHelper.GetValueSetting("Instagram"),
                //Linkedin = SettingHelper.GetValueSetting("Linkedin")
            };
        }

        public List<SitemapModel> GetDataOfSitemap()
        {
            var results = new List<SitemapModel>();
            var categories = _categoryRepository.FindAll().ToList();
            foreach (var category in categories)
            {
                if (category.Parent == null)
                {
                    results.Add(new SitemapModel
                    {
                        Title = category.Title,
                        Link = category.GetPrefixUrl(),
                        Created = DateTime.Now,
                        Priority = "1.00"
                    });
                }
                else
                {
                    results.Add(new SitemapModel
                    {
                        Title = category.Title,
                        Link = category.GetPrefixUrl(),
                        Created = DateTime.Now,
                        Priority = "0.80"
                    });
                }
            }

            var articles = _articleRepository.FindAll().ToList();
            foreach (var article in articles)
            {
                results.Add(new SitemapModel
                {
                    Title = article.Title,
                    Link = article.GetUrl(),
                    Created = DateTime.Now,
                    Priority = "0.60"
                });
            }

            return results;
        }

        public BaseResponseModel Subscribe(bool modelState, SubscribeModel model)
        {
            if (modelState)
            {
                if (!_subscribeRepository.IsExist(model.Email))
                {
                    var entity = new Subscribe()
                    {
                        Email = model.Email,
                        CreateDate = DateTime.Now,
                    };
                    _subscribeRepository.Insert(entity);

                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.OK,
                        //Message = ResourceHelper.GetResource(ResourceKeyCollection.Subscribe_success)
                    };
                }
            }

            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.BadRequest,
                //Message = ResourceHelper.GetResource(ResourceKeyCollection.Subscribe_error),
            };
        }
    }
}
