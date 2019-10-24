using MainProject.Core;
using System.Linq;

namespace MainProject.Data.Repositories
{
    public class RegionRepository : AbstractMainProjectRepository<Region>
    {
        private MainDbContext _dbContext;
        public RegionRepository(MainDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public string DeleteRegion(MainProject.Core.Region region)
        {
            _dbContext.Regions.Remove(region);
            if (_dbContext.SaveChanges() > 0)
            {
                return "Xóa thành công";
            }
            else
            {
                return "Xảy ra sự cố trong quá trình xóa dữ liệu, xin vui lòng thử lại";
            }
        }

        public bool CheckingHaveChildren(long id)
            => _dbContext.Regions.Any(x => x.Parent.Id == id);

        //public bool CheckBranches(long id)
        //    => _dbContext.Branches.Any(x => x.Region.Id == id);
    }
}
