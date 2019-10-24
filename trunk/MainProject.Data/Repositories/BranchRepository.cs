using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class BranchRepository : AbstractMainProjectRepository<Branch>
    {
        public BranchRepository(MainDbContext db)
            : base(db)
        {

        }
    }
}
