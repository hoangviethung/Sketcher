using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class LogHistoryRepository : AbstractMainProjectRepository<LogHistory>
    {
        public LogHistoryRepository(MainDbContext db)
            : base(db)
        {
            
        }
    }
}
