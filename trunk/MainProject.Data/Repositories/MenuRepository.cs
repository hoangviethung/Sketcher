using System;
using System.Collections.Generic;
using System.Linq;
using MainProject.Core;
using MainProject.Core.Enums;

namespace MainProject.Data.Repositories
{
    public class MenuRepository : AbstractMainProjectRepository<Menu>
    {
        private MainDbContext _dbContext;
        public MenuRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        public Menu FindId(long id)
        {
            return FindUnique(c => c.Id == id);
        }

        public Menu FindType(MenuTypeCollection type)
        {
            return MainDbContext.Menus.FirstOrDefault(x => x.Type == type);
        }

        public List<Menu> FindAllType(MenuTypeCollection type)
        {
            return MainDbContext.Menus.Where(x => x.Type == type).ToList();
        }
    }
}
