using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MainProject.Framework.Helper
{
    public class SettingHelper
    {
        public enum SettingKeys
        {
        }

        public SettingHelper()
        {
        }

        public static bool UpdateSettingValueByKey(string key1Table, string value1Table)
        {
            var dbContext = DalHelper.InvokeDbContext();
            var key1Setting = dbContext.Settings.FirstOrDefault(d => d.Key == key1Table);
            if (key1Setting != null && !string.IsNullOrEmpty(key1Setting.Value))
            {
                key1Setting.Value = value1Table;
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public static Dictionary<string, string> GetSettingValueByListKey(List<string> keys)
        {
            var dbContext = DalHelper.InvokeDbContext();
            var models = dbContext.Settings.Where(d => keys.Contains(d.Key)).ToList();
            return models.ToDictionary(x => x.Key, x => x.Value); ;
        }

        public static string GetValueSetting(string key1Table)
        {
            var dbContext = DalHelper.InvokeDbContext();
            var key1Setting = dbContext.Settings.FirstOrDefault(d => d.Key == key1Table);
            if (key1Setting != null && !string.IsNullOrEmpty(key1Setting.Value))
            {
                return key1Setting.Value;
            }
            return string.Empty;
        }

        public static string GetValueSetting(string key1Table, string key2Config)
        {
            var dbContext = DalHelper.InvokeDbContext();
            var key1Setting = dbContext.Settings.FirstOrDefault(d => d.Key == key1Table);
            if (key1Setting != null && !string.IsNullOrEmpty(key1Setting.Value))
            {
                return key1Setting.Value;
            }
            else
            {
                string key2Value = ConfigurationManager.AppSettings[key2Config];
                if (!string.IsNullOrEmpty(key2Value)) return key2Value;
                return string.Empty;
            }
        }
    }
}
