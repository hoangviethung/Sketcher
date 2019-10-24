using MainProject.Core.UserInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Data.Repositories
{
    public class OAuthMembershipRepository : AbstractMainProjectRepository<OAuthMembership>
    {
        public OAuthMembershipRepository(MainDbContext db)
            : base(db)
        {

        }
    }
}
