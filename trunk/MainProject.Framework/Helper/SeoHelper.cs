using System.Collections.Generic;
using System.Linq;
using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Data;

namespace MainProject.Framework.Helper
{
    public static class SeoHelper
    {
        public static string ValidateSeNameAndSubmit(EntityTypeCollection entityType, long entityId,
            string name, string title, string externalLink)
        {
            return ValidateSeNameAndSubmit(entityType, entityId, name, externalLink, null);
        }

        public static string ValidateSeNameAndSubmit(EntityTypeCollection entityType, long entityId,
            string name, string title, string externalLink, int? languageIdValue)
        {
            var dbContext = DalHelper.InvokeDbContext();

            var seName = StringHelper.GetSeName(name);
            var url = string.Empty;
            var languageId = languageIdValue.HasValue ? languageIdValue.Value : dbContext.Languages.First().Id;
            var seExists = new List<string>();
            switch (entityType)
            {
                case EntityTypeCollection.Categories:
                    var category = dbContext.Categories.FirstOrDefault(c => c.Id == entityId);
                    seExists =
                        dbContext.Categories.Where(c => c.Parent.Id == category.Parent.Id && c.Id != entityId).Select(c => c.SeName).ToList();
                    url = category.Parent != null ? category.Parent.GetPrefixUrl() : "";
                    languageId = category.Language.Id;
                    break;
                case EntityTypeCollection.Articles:
                    var article = dbContext.Articles.FirstOrDefault(c => c.Id == entityId);
                    seExists =
                        dbContext.Articles.Where(c => c.Category.Id == article.Category.Id && c.Id != entityId).Select(c => c.SeName).ToList();
                    url = article.Category.GetPrefixUrl();
                    languageId = article.Language.Id;
                    break;
            }
            var i = 1;
            var tempSeName = seName;
            var tempUrl = (url + "/" + seName).ToLower();
            var item = dbContext.UrlRecords.FirstOrDefault(c => c.Url.ToLower().Equals(tempUrl));
            while (seExists.Contains(seName) || (item != null && item.EntityId != entityId))
            {
                seName = string.Format("{0}-{1}", tempSeName, i);
                tempUrl = (url + "/" + seName);
                item = dbContext.UrlRecords.FirstOrDefault(c => c.Url.ToLower().Equals(tempUrl));
                i++;
            }
            url += "/" + seName;
            SaveToUrlRecord(dbContext, entityType, entityId, languageId, seName, url, title, externalLink);

            //update items depend on current entity
            if (entityType == EntityTypeCollection.Categories)
            {
                var category = dbContext.Categories.FirstOrDefault(c => c.Id == entityId);
                if (category != null) category.SeName = seName;
                var articleChildren = dbContext.Articles.Where(c => c.Category.Id == entityId).ToList();
                foreach (var article in articleChildren)
                {
                    var articleSeName = ValidateSeNameAndSubmit(EntityTypeCollection.Articles, article.Id, article.SeName,
                                                                article.Title, externalLink, article.Language.Id);
                    article.SeName = articleSeName;
                }

                var childCategories = dbContext.Categories.Where(c => c.Parent.Id == entityId).ToList();
                foreach (var childCategory in childCategories)
                {
                    var categorySeName = ValidateSeNameAndSubmit(entityType, childCategory.Id, childCategory.SeName,
                                                                 childCategory.Title, externalLink, childCategory.Language.Id);
                    childCategory.SeName = categorySeName;
                }
                dbContext.SaveChanges();
            }

            return seName;
        }

        private static void SaveToUrlRecord(MainDbContext dbContext, EntityTypeCollection entityType,
            long entityId, int languageId, string seName, string url, string title, string externalLink)
        {
            var urlRecord =
                dbContext.UrlRecords.FirstOrDefault(c => c.EntityType == entityType && c.EntityId == entityId);

            if (urlRecord == null)
            {
                urlRecord = new UrlRecord
                {
                    EntityId = entityId,
                    EntityType = entityType,
                    Language = dbContext.Languages.FirstOrDefault(c => c.Id == languageId),
                    SeName = seName,
                    Url = externalLink ?? url,
                    OriginUrl = url,
                    Title = title
                };
                dbContext.UrlRecords.Add(urlRecord);
            }
            else
            {
                urlRecord.SeName = seName;
                urlRecord.Url = externalLink ?? url;
                urlRecord.OriginUrl = url;
                urlRecord.Title = title;
                urlRecord.Language = dbContext.Languages.FirstOrDefault(c => c.Id == languageId);
            }
            dbContext.SaveChanges();
        }

        public static void DeleteUrlRecord(EntityTypeCollection entityType, long entityId)
        {
            var dbContext = DalHelper.InvokeDbContext();
            var urlRecords =
                dbContext.UrlRecords.Where(c => c.EntityType == entityType && c.EntityId == entityId).ToList();
            foreach (var urlRecord in urlRecords)
            {
                dbContext.UrlRecords.Remove(urlRecord);
            }
            dbContext.SaveChanges();
        }
    }
}