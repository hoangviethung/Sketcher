using System;
using System.Linq;
using System.Linq.Expressions;
using MainProject.Core.Commerce;

namespace MainProject.Data.Repositories
{
    public class ProductRepository : AbstractMainProjectRepository<Product>
    {
        private readonly MainDbContext _dbContext;
        public ProductRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        #region Admin Site
        public Product FindId(long id)
            => FindUnique(x => x.Id == id && !x.IsDeleted);

        public void Remove(Product entity)
        {
            // Delete reference
            new ProductCommerceCategoryRefRepository(_dbContext).DeleteByCriteria(x => x.Product.Id == entity.Id);
            new ProductPropertyRefRepository(_dbContext).DeleteByCriteria(x => x.Product.Id == entity.Id);
            // Delete entity
            Delete(entity);
        }
        #endregion


        #region Landing page
        public Product GetId(long id)
            => FindUnique(x => x.Id == id && !x.IsDeleted && !x.IsLocked && !x.IsHide);

        public IQueryable<Product> Get(Expression<Func<Product, bool>> predicate)
            => Find(x => !x.IsDeleted && !x.IsLocked && !x.IsHide).Where(predicate).AsQueryable();

        public IQueryable<Product> GetAll()
            => Find(x => !x.IsDeleted && !x.IsLocked && !x.IsHide); 
        #endregion
    }
}
