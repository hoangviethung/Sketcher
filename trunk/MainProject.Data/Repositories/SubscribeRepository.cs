using MainProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Data.Repositories
{
    public class SubscribeRepository : AbstractMainProjectRepository<Subscribe>
    {
        public MainDbContext _dbContext;
        public SubscribeRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }
        public Subscribe FindId(int id)
        {
            return FindUnique(c => c.Id == id);
        }

        public bool IsExist(string email)
            => _dbContext.Subscribes.Any(x => x.Email.Equals(email));
    }
}
