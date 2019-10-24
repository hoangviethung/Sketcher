using System.Linq;
using MainProject.Core.Enums;
using MainProject.Framework.Constant;

namespace MainProject.Framework.Helper
{
    public static class ResourceHelper
    {
        public static string GetResource(ResourceKeyCollection resourceKey)
        {
            var strResourceKey = resourceKey.ToString();
            return GetResource(strResourceKey);
        }

        public static string GetResource(string strResourceKey)
        {
            var culture = CultureHelper.GetCurrentLanguage();
            var dbContext = DalHelper.InvokeDbContext();
            var cacheKey = CacheConstant.GetResourceKey(strResourceKey, culture);

            var strValue = CacheHelper.GetValueCache(cacheKey);
            if (strValue == null || ((string)strValue).Equals(strResourceKey))
            {
                var stringResourceValue =
                    dbContext.StringResourceValues.FirstOrDefault(
                        c => c.Key.Name.Equals(strResourceKey) && c.Language.LanguageKey.Equals(culture));

                if (stringResourceValue != null)
                {
                    var stringValue = !string.IsNullOrEmpty(stringResourceValue.Value)
                        ? stringResourceValue.Value
                        : !string.IsNullOrEmpty(strResourceKey)
                            ? strResourceKey
                            : string.Empty;
                    CacheHelper.InsertOrUpdate(cacheKey, stringValue);
                    strValue = stringValue;
                }
            }

            return strValue != null ? (string)strValue : string.Empty;
        }

        public static void UpdateResourceOnCache(string strResourceKey, string languageKey, string value)
        {
            var cacheKey = CacheConstant.GetResourceKey(strResourceKey, languageKey);

            CacheHelper.InsertOrUpdate(cacheKey, value ?? string.Empty);
        }
    }
}
