
using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class StringResourceValueRepository:AbstractMainProjectRepository<StringResourceValue>
    {
        public StringResourceValueRepository(MainDbContext db)
            : base(db)
        {
            
        }
    }
}
