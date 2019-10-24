using System;
using MainProject.Core;
using System.Linq;

namespace MainProject.Data.Repositories
{
    public class CategoryRepository : AbstractMainProjectRepository<Category>
    {
        private MainDbContext _dbContext;
        public CategoryRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        public Category FindId(long id)
        {
            return FindUnique(c => c.Id == id);
        }

        public bool HasParent(long id)
        {
            return _dbContext.Categories.Any(d => d.Parent != null && d.Parent.Id == id);
        }

        public bool HasArticle(long id)
        {
            return _dbContext.Articles.Any(d => d.Category.Id == id);
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

        
    }
}
