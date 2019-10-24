using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class ContactRepository:AbstractMainProjectRepository<Contact>
    {
        public ContactRepository(MainDbContext db)
            : base(db)
        {
            
        }
    }
}
