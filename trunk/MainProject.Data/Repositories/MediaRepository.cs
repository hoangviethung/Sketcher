using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class MediaRepository : AbstractMainProjectRepository<Media>
    {
        public MediaRepository(MainDbContext db)
            : base(db)
        {

        }
    }
}
