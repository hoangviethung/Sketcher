using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class StringResourceKeyRepository : AbstractMainProjectRepository<StringResourceKey>
    {
        public StringResourceKeyRepository(MainDbContext db)
            : base(db)
        {
            
        }
    }
}
