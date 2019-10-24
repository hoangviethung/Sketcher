using System.Collections.Generic;
using System.Linq;
using MainProject.Core.Commerce;

namespace MainProject.Data.Repositories
{
    public class ProductPropertyRefRepository : AbstractMainProjectRepository<ProductPropertyRef>
    {
        private readonly MainDbContext _dbContext;
        public ProductPropertyRefRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        public bool Any(long id, List<string> propertyNames)
        {
            var entities = Find(x => x.Product.Id == id).ToList();

            return entities.Any(x => propertyNames.Contains(x.Value));
        }

        public void Delete(long id)
        {
            Delete(FindUnique(x => x.Id == id));
        }
    }
}
