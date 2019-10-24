using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class ImageRepository : AbstractMainProjectRepository<Image>
    {
        private MainDbContext _dbContext;
        public ImageRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }
    }
}
