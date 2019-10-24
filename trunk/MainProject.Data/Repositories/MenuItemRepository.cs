using System;
using System.Collections.Generic;
using System.Linq;
using MainProject.Core;
using MainProject.Core.Enums;

namespace MainProject.Data.Repositories
{
    public class MenuItemRepository : AbstractMainProjectRepository<MenuItem>
    {
        private MainDbContext _dbContext;
        public MenuItemRepository(MainDbContext db)
            : base(db)
        {
            _dbContext = db;
        }

        public MenuItem FindId(long id)
        {
            return FindUnique(c => c.Id == id);
        }

        public bool ExistChild(int id)
            => _dbContext.MenuItems.Any(c => c.Parent.Id == id);

        public List<MenuItem> FindMenu(Menu menu)
        {
            return MainDbContext.MenuItems.Where(x => x.Menu.Id == menu.Id).ToList();
        }

        public List<MenuItem> FindMenu(Menu menu, Language lang)
        {
            return
                MainDbContext.MenuItems.Where(x => x.Menu.Id == menu.Id && x.Language.Id == lang.Id)
                    .OrderBy(c => c.Order)
                    .ToList();
        }
        public string DeleteMenu(MenuItem menu)
        {
            _dbContext.MenuItems.Remove(menu);
            if (_dbContext.SaveChanges() > 0)
            {
                return "Xóa thành công";
            }
            else
            {
                return "Xảy ra sự cố trong quá trình xóa dữ liệu, xin vui lòng thử lại";
            }
        }
    }
}
