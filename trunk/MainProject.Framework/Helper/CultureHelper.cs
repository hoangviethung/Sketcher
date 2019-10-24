using System;
using System.Linq;
using System.Web;
using MainProject.Framework.Constant;
using MainProject.Framework.DatabaseFramework;

namespace MainProject.Framework.Helper
{
    public static class CultureHelper
    {
        public static string GetCurrentLanguage()
        {
            var currentLanguage = string.Empty;
            try
            {
                currentLanguage = (string) HttpContext.Current.Session[StringConstant.CurrentLanguageKey];
            }
            catch (Exception ex) {}
            
            if (string.IsNullOrEmpty(currentLanguage))
            {
                var dbContext = DalHelper.InvokeDbContext();
                var firstLanguage = dbContext.Languages.FirstOrDefault();
                if (firstLanguage == null)
                {
                    InitEntitiesDb.SeedEntities(dbContext);
                }
                currentLanguage = firstLanguage.LanguageKey;
                SaveCurrentLanguage(currentLanguage);
            }

            return currentLanguage;
        }

        public static int GetCurrentLanguageId()
        {
            var culKey = GetCurrentLanguage();
            var dbContext = DalHelper.InvokeDbContext();
            var language =
               dbContext.Languages.FirstOrDefault(
                   c => c.LanguageKey.Equals(culKey, StringComparison.OrdinalIgnoreCase));
            if (language == null)
            {
                throw new Exception(MessageConstant.DefaultDataNotValid);
            }
            return language.Id;
        }

        public static void SaveCurrentLanguage(string culture)
        {
            var dbContext = DalHelper.InvokeDbContext();
            var language =
                dbContext.Languages.FirstOrDefault(
                    c => c.LanguageKey.Equals(culture, StringComparison.OrdinalIgnoreCase));
            if (language == null)
            {
                language = dbContext.Languages.FirstOrDefault();
                if (language == null)
                {
                    throw new Exception(MessageConstant.DefaultDataNotValid);
                }
            }
            HttpContext.Current.Session[StringConstant.CurrentLanguageKey] = language.LanguageKey;
        }
    }
}
