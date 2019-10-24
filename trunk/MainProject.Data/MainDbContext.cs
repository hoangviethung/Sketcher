using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using MainProject.Core;
using MainProject.Core.Commerce;
using MainProject.Core.UserInfos;

namespace MainProject.Data
{
    public class MainDbContext : DbContext
    {
        public MainDbContext() : base("DefaultConnection") { }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<MembershipItem> MembershipItems { get; set; }

        public DbSet<OAuthMembership> OAuthMemberships { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<LogHistory> LogHistories { get; set; }

        public DbSet<StringResourceKey> StringResourceKeys { get; set; }

        public DbSet<StringResourceValue> StringResourceValues { get; set; }

        public DbSet<UrlRecord> UrlRecords { get; set; }
        
        public DbSet<Setting> Settings { get; set; }

        public DbSet<Region> Regions { get; set; } 

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; } 
        
        public DbSet<Role> Roles { get; set; }

        public  DbSet<RolePrivillege> RolePrivilleges { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Media> Medias { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<FaqItem> FaqItems { get; set; }

        public DbSet<ContactEmail> ContactEmails { get; set; }

        public DbSet<Image> Images { get; set; }

        #region E-Commerce
        public DbSet<CommerceCategory> CommerceCategories { get; set; }

        public DbSet<CommerceCategoryTranslation> CommerceCategoryTranslations { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCommerceCategoryRef> ProductCommerceCategoryRefs { get; set; }

        // Product Properties
        public DbSet<Property> Properties { get; set; }

        public DbSet<ProductPropertyRef> ProductPropertyRefs { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInRole>().HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<UserInRole>()
                    .HasRequired<UserProfile>(s => s.User)
                    .WithMany(s => s.UserInRoles)
                    .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<OAuthMembership>().HasKey(x => new { x.Provider, x.ProviderUserId, x.UserId });

            modelBuilder.Entity<MembershipItem>()
                .Property(x => x.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
