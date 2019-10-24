using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class UrlRecordRepository : AbstractMainProjectRepository<UrlRecord>
    {
        public UrlRecordRepository(MainDbContext db)
            : base(db)
        {
        }
    }
}
