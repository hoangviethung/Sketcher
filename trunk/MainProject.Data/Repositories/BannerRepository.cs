using MainProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Data.Repositories
{
    public class BannerRepository : AbstractMainProjectRepository<Banner>
    {
        public MainDbContext _dbContext;
        public BannerRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        public Banner FindById(long Id)
        {
            return _dbContext.Banners.Find(Id);
        }
    }
}
