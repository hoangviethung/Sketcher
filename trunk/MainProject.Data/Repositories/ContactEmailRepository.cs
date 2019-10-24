using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class ContactEmailRepository : AbstractMainProjectRepository<ContactEmail>
    {
        private MainDbContext _dbContext;
        public ContactEmailRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }
        
    }
}
