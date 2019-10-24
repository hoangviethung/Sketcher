using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Framework.Constant;
using WebMatrix.WebData;
using MainProject.Framework.Helper;

namespace MainProject.Framework.DatabaseFramework
{
    public class InitEntitiesDb : IDatabaseInitializer<MainDbContext>
    {
        #region IDatabaseInitializer<> Members

        public void InitializeDatabase(MainDbContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                // create all tables
                var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                context.Database.ExecuteSqlCommand(dbCreationScript);

                //create membership tables
                if (!WebSecurity.Initialized)
                {
                    WebSecurity.InitializeDatabaseConnection("MainDbContext",
                        "UserProfile", "UserId", "UserName", autoCreateTables: true);
                }

                SeedEntities(context);
                SeedMembership();
            }
            else
            {
                throw new ApplicationException("No database instance");
            }
        }

        public void InitMainDatabase()
        {
            var dbContext = new MainDbContext();
            if (!dbContext.Database.Exists())
            {
                dbContext.Database.Create();
                SeedEntities(dbContext);
            }
        }

        public void InitUserDatabase()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("MainDbContext",
                    "UserProfile", "UserId", "UserName", autoCreateTables: true);
                SeedMembership();
            }
        }

        #endregion

        #region Methods

        public static void SeedEntities(MainDbContext context)
        {
            //context.Languages.Add(new Language
            //{
            //    LanguageKey = "vi",
            //    LanguageName = "Vietnamese"
            //});
            //context.Languages.Add(new Language
            //{
            //    LanguageKey = "en",
            //    LanguageName = "English"
            //});
            //context.SaveChanges();

            //var defLang = context.Languages.FirstOrDefault();
            //context.Categories.Add(new Category
            //{
            //    EntityType = EntityTypeCollection.Categories,
            //    IsSystem = true,
            //    Language = defLang,
            //    Order = 0,
            //    OriginalValue = Guid.NewGuid(),
            //    SeName = "root-news-category"
            //});
            ////context.Categories.Add(new Category
            ////{
            ////    EntityType = EntityTypeCollection.AttachmentCategories,
            ////    IsSystem = true,
            ////    Language = defLang,
            ////    Order = 0,
            ////    OriginalValue = Guid.NewGuid(),
            ////    SeName = "root-attachments-category"
            ////});
            //context.SaveChanges();

            //var mainmenu = new Menu() { Type = MenuTypeCollection.Top, CodeName = "main-menu", Order = 1 };
            //var arrMnuItemName = new string[] { "SẢN PHẨM", "HỎI ĐÁP", "TIN TỨC", "CHÍNH SÁCH", "HỔ TRỢ", "LIÊN HỆ" };
            //foreach (var s in arrMnuItemName)
            //{
            //    context.MenuItems.Add(new MenuItem()
            //    {
            //        Language = defLang,
            //        OriginalValue = Guid.NewGuid(),
            //        Title = s,
            //        Link = "javascript:void(0)",
            //        LinkTarget = LinkTargetCollection.Self,
            //        Menu = mainmenu
            //    });
            //}

            //var bottomMenus = new Dictionary<string, List<string>>();
            //bottomMenus.Add("Thông tin home shopping", new List<string>() { "Giới thiệu Home Shopping", "Tiêu chí bán hàng", "Đối tác chiến lược" });
            //bottomMenus.Add("Chính sách", new List<string>() { "Chính sách giao hàng", "Khu vực giao nhận miễn phí", "Chính sách thẻ khách hàng" });
            //bottomMenus.Add("Hỗ trợ khách hàng", new List<string>() { "Hướng dẫn mua hàng", "Hình thức thanh toán", "Chăm sóc khách hàng", "Đổi trả sản phẩm" });
            //bottomMenus.Add("Thông tin", new List<string>() { "Liên hệ", "Hợp tác", "Bảo mật thông tin", "Sơ đồ website" });
            //foreach (var m in bottomMenus)
            //{
            //    var menu = new Menu() { Type = MenuTypeCollection.Bottom, CodeName = StringHelper.GetSeName(m.Key), Order = 1 };
            //    foreach (var mitem in m.Value)
            //    {
            //        context.MenuItems.Add(new MenuItem()
            //        {
            //            Language = defLang,
            //            OriginalValue = Guid.NewGuid(),
            //            Title = mitem,
            //            Link = "javascript:void(0)",
            //            LinkTarget = LinkTargetCollection.Self,
            //            Menu = menu
            //        });
            //    }
            //}
            
            //context.SaveChanges();
        }

        public static void SeedMembership()
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            //var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!roles.RoleExists(RoleName.Admin))
            {
                roles.CreateRole(RoleName.Admin);
            }
            //if (!roles.RoleExists(RoleName.Mod))
            //{
            //    roles.CreateRole(RoleName.Mod);
            //}
            //if (!roles.RoleExists(RoleName.Guest))
            //{
            //    roles.CreateRole(RoleName.Guest);
            //}
            //membership.DeleteAccount(StringConstant.DefaultAdministrator);
            //membership.DeleteUser(StringConstant.DefaultAdministrator, true);
            //membership.CreateUserAndAccount(StringConstant.DefaultAdministrator, "@123456",
            //        new Dictionary<string, object>
            //        {
            //            {"IsActive", true}
            //        });
            //if (!roles.GetRolesForUser(StringConstant.DefaultAdministrator).ToList().Contains(RoleName.Admin))
            //{
            //    roles.AddUsersToRoles(new[] { StringConstant.DefaultAdministrator }, new[] { RoleName.Admin });
            //}
        }
        #endregion
    }
}