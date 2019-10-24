using MainProject.Core;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.Service.Model;
using System.Collections.Generic;
using System.Linq;

namespace MainProject.Service.Service.Search
{
    public class SearchService
    {
        #region Fields
        private readonly ArticleRepository _articleRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly LanguageRepository _languageRepository;
        private int searchPageItems;
        #endregion
        
        #region Constructor
        public SearchService(MainDbContext DbContext)
        {
            _articleRepository = new ArticleRepository(DbContext);
            _categoryRepository = new CategoryRepository(DbContext);
            _languageRepository = new LanguageRepository(DbContext);
            searchPageItems = 20;
        }
        #endregion

        public SearchViewModel Search(string text, int page, int languageId)
        {
            if (page < 1) page = 1;
            var textS = text.Trim().ToLower();
            var searchObjects = new List<SearchObject>();

            var newsArts = _articleRepository.Find(
                    x => x.Title.Contains(textS) || x.Description.Contains(textS) || x.Body.Contains(textS))
                    .Where(x => x.Language.Id == languageId && x.IsPublished)
                    .OrderByDescending(x => x.Id).ToList();

            foreach (var newsArt in newsArts)
            {
                var newsSearchObject = new SearchObject
                {
                    Image = newsArt.ImageDefault,
                    Name = newsArt.Title,
                    Description = newsArt.Description,
                    Url = newsArt.GetUrl(),
                    CreadtedDate = newsArt.CreateDate,
                    ViewCount = newsArt.ViewCount
                };
                searchObjects.Add(newsSearchObject);
            }

            var count = searchObjects.Count();

            var SearchObjects = searchObjects.Skip((page - 1) * searchPageItems).Take(searchPageItems).ToList();

            var searchViewModel = new SearchViewModel()
            {
                SearchObjects = SearchObjects,
                PagingModel = new PagingModel(count, searchPageItems, page, "href='/Search?text=" + text + "&page=" + "{0}'"),
                TotalResults = searchObjects.Count,
                Text = text
            };

            return searchViewModel;
        }
    }
}
