using MainProject.Core.Commerce;
using System.Linq;

namespace MainProject.Data.Repositories
{
    public class CommerceCategoryRepository : AbstractMainProjectRepository<CommerceCategory>
    {
        private MainDbContext _dbContext;
        public CommerceCategoryRepository(MainDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public bool ExistChild(long id)
            => _dbContext.CommerceCategories.Any(x => x.Parent.Id == id);

    }
}
