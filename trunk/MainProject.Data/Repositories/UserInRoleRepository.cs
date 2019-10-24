using MainProject.Core.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Data.Repositories
{
    public class UserInRoleRepository : AbstractMainProjectRepository<UserInRole>
    {
        public UserInRoleRepository(MainDbContext db)
            : base(db)
        {

        }
    }
}
