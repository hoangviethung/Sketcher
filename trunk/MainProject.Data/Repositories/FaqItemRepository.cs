using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class FaqItemRepository : AbstractMainProjectRepository<FaqItem>
    {
        private MainDbContext _dbContext;
        public FaqItemRepository(MainDbContext db): base(db)
        {
            _dbContext = db;
        }
    }
}
