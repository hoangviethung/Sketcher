using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Helper;
using MainProject.Web.Areas.Admin.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Service.Common
{
    public class CommonService
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly MenuRepository _menuRepository;
        private readonly MenuItemRepository _menuItemRepository;

        public CommonService(MainDbContext dbContext)
        {
            _categoryRepository = new CategoryRepository(dbContext);
            _menuRepository = new MenuRepository(dbContext);
            _menuItemRepository = new MenuItemRepository(dbContext);
        }

        /// <summary>
        /// Get categories by language
        /// </summary>
        /// <param name="id">Language id</param>
        /// <param name="cateId">Category id</param>
        /// <param name="isFilterTemplate">Filter category by display template</param>
        /// <returns></returns>
        public List<SelectListItem> GetCategories(int id, long cateId = 0)
            => CategoryHelper.BindSelectListItem(_categoryRepository, cateId, id, 0, true);


        public List<SelectListItem> GetMenus(int langId, int selectedValue = 0)
            => _menuRepository.FindAll().OrderBy(n => n.Order)
                                        .Select(x => new SelectListItem
                                        {
                                            Text = x.CodeName,
                                            Value = x.Id.ToString()
                                        }).ToList();

        public List<SelectListItem> GetMenuItems(int langId, int menuId, int selectedValue = 0)
            => MenuItemHelper.BindDropdown(_menuItemRepository.Find(x => x.Menu.Id == menuId
                                                                      && x.Language.Id == langId).ToList(), selectedValue);
    }
}
