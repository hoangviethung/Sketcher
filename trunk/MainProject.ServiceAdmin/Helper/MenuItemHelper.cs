using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Menu;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Helper
{
   public class MenuItemHelper
    {
        /// <summary>
        /// Bind drop down list for select menu
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="selectedvalue"></param>
        /// <returns></returns>
        public static List<SelectListItem> BindDropdown(IList<MenuItem> menus, long selectedvalue)
        {
            // Initialize model
            var model = new List<SelectListItem>();
            if (menus.All(c => c.Id != 0))
            {
                menus.Insert(0, new MenuItem
                {
                    Title = "Menu gốc",
                    Id = 0
                });
            }
            // Get root menu item
            var menuItem = menus.Where(x => x.Parent == null).OrderBy(x => x.Order).ToList();

            // Bind select list for child
            foreach (var item in menuItem)
            {
                // Bind data to model
                var selectListItem = new SelectListItem
                {
                    Selected = item.Id == selectedvalue,
                    Text = item.Title,
                    Value = item.Id.ToString()
                };
                // Add model to list item
                model.Add(selectListItem);
                // Add child category model to list item
                model.AddRange(BuildSelectListItemsOfChild(menus, item.Id, selectedvalue, selectListItem.Text));
            }

            return model;
        }

        private static IList<SelectListItem> BuildSelectListItemsOfChild(IList<MenuItem> menus, long menuId, long? selectedvalue, string prefix)
        {
            var listResult = new List<SelectListItem>();
            // Get all category has parent is root
            var menuItem = menus.Where(x => x.Parent != null && x.Parent.Id == menuId).OrderBy(x => x.Order).ToList();
            // Loop child categories for binding data to model
            foreach (var item in menuItem)
            {
                // Bind data to model
                var selectListItem = new SelectListItem
                {
                    Selected = item.Id == selectedvalue,
                    Text = prefix + " >> " + item.Title,
                    Value = item.Id.ToString()
                };
                // Add model to list item
                listResult.Add(selectListItem);
                // Add child category model to list item
                listResult.AddRange(BuildSelectListItemsOfChild(menus, item.Id, selectedvalue, selectListItem.Text));
            }

            return listResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuItemRepository"></param>
        /// <param name="menuRepository"></param>
        /// <param name="languageRepository"></param>
        /// <param name="model"></param>
        /// <param name="_logHistoryService"></param>
        /// <returns></returns>
        public static bool CreateOrUpdate(MenuItemRepository menuItemRepository, MenuRepository menuRepository, LanguageRepository languageRepository, MenuItemManageModel model , LogHistoryService _logHistoryService)
        {
            MenuItem menuItem = null;
            // Create
            if (model.Id == 0)
            {
                menuItem = new MenuItem();
                // Bind data from model to entity
                MenuItemManageModel.ToEntity(model, ref menuItem);
                menuItem.Language = languageRepository.FindUnique(c => c.Id == model.LanguageSelectedValue);
                menuItem.Menu = menuRepository.FindId(model.MenuSelectedValue);
                menuItem.Parent = menuItemRepository.FindUnique(c => c.Id == model.ParentSelectedId);
                // Insert menu item to db
                menuItemRepository.Insert(menuItem);
                //save loghistory
                _logHistoryService.Update(model.LogHistoryId, menuItem.Id);
            }
            else // Edit
            {
                // Get menu item to update
                menuItem = menuItemRepository.FindUnique(c => c.Id == model.Id);
                if (menuItem == null) return false;
                // Bind data from model to entity
                MenuItemManageModel.ToEntity(model, ref menuItem);
                menuItem.Language = languageRepository.FindUnique(c => c.Id == model.LanguageSelectedValue);
                menuItem.Menu = menuRepository.FindId(model.MenuSelectedValue);
                menuItem.Parent = menuItemRepository.FindId(model.ParentSelectedId);
                // update entity
                menuItemRepository.SaveChanges();
                //save logHistory
                _logHistoryService.Create(new LogHistoryModel() { EntityId = menuItem.Id, ActionType = ActionTypeCollection.Edit });
            }

            return true;
        }

        ///// <summary>
        ///// Bind model for menu item
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <param name="entity"></param>
        ///// <param name="items"></param>
        ///// <param name="_menuItemRepository"></param>
        //public static List<MenuItemModel> BuildModels(IList<MenuItem> entities, MenuItemRepository _menuItemRepository)
        //{
        //    var items = new List<MenuItemModel>();
        //    foreach (var entity in entities)
        //    {
        //        // Bind menu item to model
        //        items.Add(new MenuItemModel
        //        {
        //            Id = entity.Id,
        //            Title = entity.Title,
        //            Order = entity.Order,
        //            ParentPath = entity.GetParent(),
        //            Language = entity.Language,
        //        });

        //        var childItems = entities.Where(c => c.Parent != null && c.Parent.Id == entity.Id).OrderByDescending(c => c.Order);
        //        foreach (var childItem in childItems)
        //        {
        //            // Bind child menu item to model
        //            BuildModels(entities, childItem, ref items, _menuItemRepository);
        //        }
        //    }
        //}
    }
}
