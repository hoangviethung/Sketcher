using System;
using System.Data.Entity;
using System.Web;
using MainProject.Data;
using MainProject.Framework.Constant;

namespace MainProject.Framework.Helper
{
    public static class DalHelper
    {
        public static MainDbContext InitDbContext()
        {
            Database.SetInitializer<MainDbContext>(null);
            var dbContext = new MainDbContext();
            dbContext.Database.CreateIfNotExists();

            return dbContext;
        }

        private static MainDbContext GetDbContextFromRequest()
        {
            if (HttpContext.Current.Items.Contains(StringConstant.DbContextKeyName))
            {
                try
                {
                    var dbContext = (MainDbContext)HttpContext.Current.Items[StringConstant.DbContextKeyName];
                    return dbContext;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public static void SaveDbContextToRequest(MainDbContext dbContext)
        {
            if (HttpContext.Current.Items.Contains(StringConstant.DbContextKeyName))
            {
                HttpContext.Current.Items[StringConstant.DbContextKeyName] = dbContext;
            }
            else
            {
                HttpContext.Current.Items.Add(StringConstant.DbContextKeyName, dbContext);
            }
        }

        public static MainDbContext InvokeDbContext()
        {
            var dbContext = GetDbContextFromRequest();
            if (dbContext == null)
            {
                dbContext = InitDbContext();
                SaveDbContextToRequest(dbContext);
            }

            return dbContext;
        }

        public static void ReleaseDbContextOnRequest()
        {
            if (HttpContext.Current.Items.Contains(StringConstant.DbContextKeyName))
            {
                try
                {
                    var dbContext = (MainDbContext)HttpContext.Current.Items[StringConstant.DbContextKeyName];
                    dbContext.Dispose();
                }
                catch (Exception) {}

                HttpContext.Current.Items.Remove(StringConstant.DbContextKeyName);
            }
        }
    }
}
