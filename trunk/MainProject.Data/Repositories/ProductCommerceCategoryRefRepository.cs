using MainProject.Core.Commerce;
using System.Linq;

namespace MainProject.Data.Repositories
{
    public class ProductCommerceCategoryRefRepository : AbstractMainProjectRepository<ProductCommerceCategoryRef>
    {
        private readonly MainDbContext _dbContext;
        public ProductCommerceCategoryRefRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        /// <summary>
        /// Get active product by commerce category or get all product incase id = 0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<ProductCommerceCategoryRef> GetProductByCategory(long id)
            => Find(x => id != 0 ? (x.CommerceCategory.Id == id || x.CommerceCategory.Parent.Id == id
                        || x.CommerceCategory.Parent.Parent.Id == id) : true && !x.Product.IsDeleted
                                && !x.Product.IsLocked && !x.Product.IsHide);
    }
}
