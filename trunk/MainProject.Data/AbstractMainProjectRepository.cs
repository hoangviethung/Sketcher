using VgFramework.Data.EntityFramework;

namespace MainProject.Data
{
    public class AbstractMainProjectRepository<T> : AbstractRepository<T> where T : class
    {
        protected readonly MainDbContext MainDbContext;
        protected AbstractMainProjectRepository(MainDbContext db) : base(db)
        {
            Db = MainDbContext = db;
        }
    }
}
