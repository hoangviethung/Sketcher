using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class ArticleRepository:AbstractMainProjectRepository<Article>
    {
        private MainDbContext _dbContext;
        public ArticleRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        public Article FindId(long Id)
        {
            return FindUnique(x => x.Id == Id && x.IsPublished);
        }
    }
}
