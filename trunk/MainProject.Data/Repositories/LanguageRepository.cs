using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MainProject.Core;

namespace MainProject.Data.Repositories
{
    public class LanguageRepository : AbstractMainProjectRepository<Language>
    {
        private MainDbContext _dbContext;
        public LanguageRepository(MainDbContext db): base(db)
        {
            _dbContext = db;
        }

        public Language FindId(int id)
        {
            return _dbContext.Languages.Find(id);
        }

        public bool Update(Language language)
        {
            _dbContext.Entry(language).State = EntityState.Modified;
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
