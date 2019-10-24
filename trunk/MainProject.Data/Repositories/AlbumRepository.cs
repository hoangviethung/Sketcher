using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class AlbumRepository : AbstractMainProjectRepository<Album>
    {
        public AlbumRepository(MainDbContext db)
            : base(db)
        {

        }
    }
}
