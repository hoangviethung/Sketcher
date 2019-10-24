using System;
using System.Linq;
using MainProject.Data;

namespace MainProject.Framework.Helper
{
    public static class ConfigItemHelper
    {
        public static long GetRootProductCategoryId()
        {
            return 0;
        }

        public static long GetRootNewsId()
        {
            var rootCategoryIdStr = System.Configuration.ConfigurationManager.AppSettings["rootNewsId"];
            long rootCategoryId = 0;
            try
            {
                rootCategoryId = Convert.ToInt64(rootCategoryIdStr);
            }
            catch (Exception) { }

            return rootCategoryId;
        }

        public static int GetMaxFileUploadSize()
        {
            var itemsCountStr = System.Configuration.ConfigurationManager.AppSettings["fileUploadSize"];
            var itemsCount = 6096;
            try
            {
                itemsCount = Convert.ToInt32(itemsCountStr);
            }
            catch (Exception) { }

            return itemsCount;
        }

        public static bool IsMaintainance()
        {
            bool isMaintainance = false;
            try
            {
                isMaintainance = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["IsMaintainance"]);
            }
            catch (Exception) { }

            return isMaintainance;
        }
    }
}
