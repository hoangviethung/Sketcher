using MainProject.Core.Commerce;

namespace MainProject.Data.Repositories
{
    public class PropertyRepository : AbstractMainProjectRepository<Property>
    {
        public PropertyRepository(MainDbContext db)
            : base(db)
        {
        }
    }
}
