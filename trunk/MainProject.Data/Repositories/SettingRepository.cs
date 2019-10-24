using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class SettingRepository : AbstractMainProjectRepository<Setting>
    {
        private MainDbContext _dbContext;

        public SettingRepository(MainDbContext db): base(db)
        {
            _dbContext = db;
        }

        public bool CheckDuplicateKey(string key)
        {
            var model = FindUnique(d => d.Key == key);
            if(model!=null)
            {
                return false;
            }
            return true;
        }

        public bool CheckDuplicateKey(string key,int id)
        {
            var model = FindUnique(d => d.Key == key && d.Id!=id);
            if (model == null)
            {
                return true;
            }
            return false;
        }
    }
}
