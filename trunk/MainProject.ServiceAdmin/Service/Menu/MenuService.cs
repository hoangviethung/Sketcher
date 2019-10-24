using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Menu;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Menu
{
    public class MenuService
    {
        private readonly MenuItemRepository _menuItemRepository;
        private readonly MenuRepository _menuRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LogHistoryService _logHistoryService;

        public MenuService(MainDbContext dbContext, string currentUser)
        {
            _menuItemRepository = new MenuItemRepository(dbContext);
            _menuRepository = new MenuRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Menu);
        }

        /// <summary>
        /// Filter menu items for Index View
        /// </summary>
        /// <param name="menuSelectedId"></param>
        /// <param name="languageSelectedId"></param>
        /// <returns></returns>
        public IndexViewModel<MenuItemModel> GetIndex(int menuSelectedId = 0, int languageSelectedId = 0)
        {
            // Check languageId is exist
            languageSelectedId = languageSelectedId == 0 ? _languageRepository.FindAll().FirstOrDefault().Id : languageSelectedId;
            // Get menu items by menu and language filter
            var query =  _menuItemRepository.Find(c => c.Language.Id == languageSelectedId);
            // filter by menu
            if (menuSelectedId != 0)
            {
                query = query.Where(x => x.Menu.Id == menuSelectedId);
            }

            return new IndexViewModel<MenuItemModel>() {
                ListItems = query.ToList().Select(x => new MenuItemModel(x)).ToList(),
                FilterViewModel = new FilterViewModel()
                {
                    LanguageViewModel = new LanguageSelectModel()
                    {
                        Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), languageSelectedId),
                        LanguageSelectedValue = languageSelectedId
                    },
                    FatherSelectModel = new FatherSelectModel()
                    {
                        Fathers = MenuHelper.BindDropdown(_menuRepository.FindAll().OrderBy(x => x.Order).ToList(), menuSelectedId),
                        FatherSelectedValue = menuSelectedId
                    },
                    BaseUrl = "/Admin/MenuAdmin/Index?"
                }
            };
        }

        /// <summary>
        /// Prepare data for Create View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuItemManageModel Create()
        {
            // Bind model
            MenuItemManageModel model = InitManageModel(null);
            // Save log history
            model.LogHistoryId = _logHistoryService.Create(new LogHistoryModel() { ActionType = ActionTypeCollection.Temp });

            return model;
        }

        /// <summary>
        /// Prepare data for Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuItemManageModel Edit(long id)
        {
            var entity = _menuItemRepository.FindUnique(c => c.Id == id);
            var model = InitManageModel(entity);
            model.IsEdit = true;

            return model;
        }

        /// <summary>
        /// Insert or Update entity to db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Manage(MenuItemManageModel model)
        {
            return MenuItemHelper.CreateOrUpdate(_menuItemRepository, _menuRepository,
                    _languageRepository, model,_logHistoryService);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MenuItemManageModel ReInitManageModel(MenuItemManageModel model)
        {
            model.LanguageSelectListItems = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), model.LanguageSelectedValue);
            model.LinkTargetSelectListItems = EnumBidingHelper.LinkTargetSelectListItems(model.LinkTargetSelected);
            model.MenuSelectListItems = MenuHelper.BindDropdown(_menuRepository.FindAll().ToList(), model.MenuSelectedValue);

            var menuItems = _menuItemRepository.Find(c => c.Menu.Id == model.MenuSelectedValue && c.Language.Id == model.LanguageSelectedValue);
            model.Parents = MenuItemHelper.BindDropdown(menuItems.ToList(), model.ParentSelectedId);

            return model;
        }

        private MenuItemManageModel InitManageModel(MenuItem entity)
        {
            var model = new MenuItemManageModel();
            var languages = _languageRepository.FindAll().ToList();
            // Bind data for Edit model
            if (entity != null)
            {
                model = new MenuItemManageModel(entity);
                model.LanguageSelectListItems = LanguageHelper.BindDropdown(languages, model.LanguageSelectedValue);
                model.LinkTargetSelectListItems = EnumBidingHelper.LinkTargetSelectListItems(model.LinkTargetSelected);
                model.MenuSelectListItems = MenuHelper.BindDropdown(_menuRepository.Find(c => c.Id != model.Id).ToList(), model.MenuSelectedValue);
                var menuItems = _menuItemRepository.Find(c => c.Menu.Id == entity.Menu.Id && c.Language.Id == entity.Language.Id && c.Id != entity.Id);
                model.Parents = MenuItemHelper.BindDropdown(menuItems.OrderBy(o => o.Order).ToList(), model.ParentSelectedId);
            }
            else // Bind data for Create model
            {
                model.LanguageSelectListItems = LanguageHelper.BindDropdown(languages, model.LanguageSelectedValue);
                model.LinkTargetSelectListItems = EnumBidingHelper.LinkTargetSelectListItems(model.LinkTargetSelected);
                model.MenuSelectListItems = MenuHelper.BindDropdown(_menuRepository.FindAll().OrderBy(x => x.Order).ToList(),
                                                                    model.MenuSelectedValue);
                var menuItems =
                    _menuItemRepository.Find(c => c.Menu.Id == model.MenuSelectedValue && c.Language.Id == model.LanguageSelectedValue)
                        .ToList();
                model.Parents = MenuItemHelper.BindDropdown(menuItems, 0);
            }

            return model;
        }

        public BaseResponseModel<long[]> Delete(int id)
        {
            // Get entity to delete
            var entity = _menuItemRepository.FindUnique(c => c.Id == id);
            // Check entity is exist
            if (entity != null)
            {
                // Get filter param to redirect Index
                var menuSelectedId = entity.Menu.Id;
                var cul = entity.Language.Id;
                // Check entity has children
                if (_menuItemRepository.ExistChild(id))
                {
                    return new BaseResponseModel<long[]>
                    {
                        Code = HttpStatusCodeCollection.OK,
                        Message = string.Format("<strong style='color:red'>{0} có nhóm con, không thể xóa</strong>", entity.Title),
                        Result = new[] { menuSelectedId, cul },
                    };
                }
                else
                {
                    return new BaseResponseModel<long[]>
                    {
                        Code = HttpStatusCodeCollection.OK,
                        Message = string.Format("<strong style='color:green'>{0}</strong>", _menuItemRepository.DeleteMenu(entity)),
                        Result = new[] { menuSelectedId, cul }
                    };               
                }
              
            }
            return new BaseResponseModel<long[]>
            {
                Message = string.Format("<strong style='color:red'>Không tìm thấy đối tượng cần xóa</strong>"),
                Code = HttpStatusCodeCollection.BadRequest
            };

        }
    }
}
