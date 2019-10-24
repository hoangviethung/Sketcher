using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MainProject.Service.Helper
{
    public static class MenuHelper
    {
        public static string GetMenu(Menu menu, Language language, MenuItemRepository menuRepository, bool isMobile)
            => !isMobile ? BuildMenu(menu, language, menuRepository) : BuildMenuMobile(menu, language, menuRepository);

        public static string GetFooterMenu(Menu menu, Language language, MenuItemRepository menuRepository, bool isMobile)
            => !isMobile ? BuildFooterMenu(menu, language, menuRepository) : BuildFooterMenuMobile(menu, language, menuRepository);

        #region PC
        private static string BuildMenu(Menu menu, Language language, MenuItemRepository menuRepository)
        {
            var result = string.Empty;
            if (menu != null)
            {
                var menuItems = menuRepository.FindMenu(menu, language);
                var strListLi = string.Empty;
                foreach (var menuItem in menuItems.Where(x => x.Parent == null))
                {
                    strListLi += BuildMenuItem(menuItem, menuItems);
                }
                result = string.Format("<ul class={0}>{1}</ul>", menu.CodeName, strListLi);
            }
            return result;
        }

        private static string BuildMenuItem(MenuItem mnuItem, List<MenuItem> list)
        {
            var str = string.Empty;
            if (mnuItem != null)
            {
                var linkTarget = mnuItem.LinkTarget == LinkTargetCollection.Self ? "" : mnuItem.LinkTarget.ToString();
                if (list.Any(x => x.Parent != null && x.Parent.Id == mnuItem.Id))
                {
                    var strChilds = string.Empty;
                    foreach (
                        var m in
                            list.Where(x => x.Parent != null && x.Parent.Id == mnuItem.Id)
                                .OrderByDescending(c => c.Order))
                    {
                        strChilds += BuildMenuItem(m, list);
                    }
                    str = string.Format("<li><a href=\"{0}\" target=\"{1}\">{2}</a><ul>{3}</ul></li>", mnuItem.Link, linkTarget, mnuItem.Title, strChilds);
                }
                else
                {
                    str = string.Format("<li><a href=\"{0}\" target=\"{1}\">{2}</a></li>", mnuItem.Link, linkTarget, mnuItem.Title);
                }
            }
            return str;
        }

        private static string BuildFooterMenu(Menu menu, Language language, MenuItemRepository menuRepository)
        {
            var result = string.Empty;
            if (menu != null)
            {
                var menuItems = menuRepository.FindMenu(menu, language);
                var strListLi = string.Empty;
                foreach (var menuItem in menuItems.Where(x => x.Parent == null).OrderBy(x => x.Order))
                {
                    var level = 0;
                    strListLi += string.Format("<div class=\"item-mainft\"><h3>{0}</h3><ul>{1}</ul></div>"
                                                , menuItem.Title
                                                , BuildFooterMenuItem(menuItem, menuItems, ref level));
                }

                result = string.Format("{0}", strListLi);
            }
            return result;
        }

        private static string BuildFooterMenuItem(MenuItem mnuItem, List<MenuItem> list, ref int level)
        {
            level++;
            var str = string.Empty;
            if (mnuItem != null)
            {
                var linkTarget = mnuItem.LinkTarget == LinkTargetCollection.Self ? "" : mnuItem.LinkTarget.ToString();
                if (list.Any(x => x.Parent != null && x.Parent.Id == mnuItem.Id))
                {

                    var strChilds = string.Empty;
                    foreach (var m in list.Where(x => x.Parent != null && x.Parent.Id == mnuItem.Id).OrderBy(c => c.Order))
                    {
                        strChilds += BuildFooterMenuItem(m, list, ref level);
                        level--;
                    }
                    str = strChilds;
                }
                else
                {
                    if (level != 1)
                        str = string.Format("<li><a href=\"{0}\" target=\"{1}\">{2}</a></li>", !string.IsNullOrEmpty(mnuItem.Link) ? mnuItem.Link : "javascript:void(0)", linkTarget, mnuItem.Title);
                }
            }
            return str;
        }
        #endregion

        #region Mobile
        private static string BuildMenuMobile(Menu menu, Language language, MenuItemRepository menuRepository)
        {
            var result = string.Empty;
            if (menu != null)
            {
                var menuItems = menuRepository.FindMenu(menu, language);
                var strListLi = string.Empty;
                foreach (var menuItem in menuItems.Where(x => x.Parent == null))
                {
                    var level = 0;
                    strListLi += BuildMenuItemMobile(menuItem, menuItems, ref level);

                }
                result = string.Format("<ul>{0}</ul>", strListLi);
            }
            return result;
        }

        private static string BuildMenuItemMobile(MenuItem mnuItem, List<MenuItem> list, ref int level)
        {
            level++;
            var str = string.Empty;
            var img = string.Empty;
            if (mnuItem != null)
            {
                var linkTarget = mnuItem.LinkTarget == LinkTargetCollection.Self ? "" : mnuItem.LinkTarget.ToString();
                if (list.Any(x => x.Parent != null && x.Parent.Id == mnuItem.Id))
                {
                    var strChilds = string.Empty;
                    foreach (var m in list.Where(x => x.Parent != null && x.Parent.Id == mnuItem.Id).OrderBy(c => c.Order))
                    {
                        strChilds += BuildMenuItemMobile(m, list, ref level);
                        level--;
                    }

                    str = string.Format("<a href=\"{0}\" target=\"{1}\">{2}</a><span></span><ul>{3}</ul>",
                        !string.IsNullOrEmpty(mnuItem.Link) ? mnuItem.Link : "javascript:void(0)",
                        linkTarget,
                        mnuItem.Title,
                        strChilds);
                    str = string.Format("<li>{0}</li>", str);
                }
                else
                {
                    str = string.Format("<li><a href=\"{0}\" target=\"{1}\">{2}</a></li>",
                        !string.IsNullOrEmpty(mnuItem.Link) ? mnuItem.Link : "javascript:void(0)",
                        linkTarget, mnuItem.Title);
                }
            }

            return str;
        }

        private static string BuildFooterMenuMobile(Menu menu, Language language, MenuItemRepository menuRepository)
        {
            var result = string.Empty;
            if (menu != null)
            {
                var menuItems = menuRepository.FindMenu(menu, language);
                var strListLi = string.Empty;
                foreach (var menuItem in menuItems.Where(x => x.Parent == null))
                {
                    var level = 0;
                    strListLi += BuildFooterMenuItemMobile(menuItem, menuItems, ref level);

                }
                result = string.Format("<ul>{0}</ul>", strListLi);
            }
            return result;
        }

        private static string BuildFooterMenuItemMobile(MenuItem mnuItem, List<MenuItem> list, ref int level)
        {
            level++;
            var str = string.Empty;
            var img = string.Empty;
            if (mnuItem != null)
            {
                var linkTarget = mnuItem.LinkTarget == LinkTargetCollection.Self ? "" : mnuItem.LinkTarget.ToString();
                if (list.Any(x => x.Parent != null && x.Parent.Id == mnuItem.Id))
                {
                    var strChilds = string.Empty;
                    foreach (var m in list.Where(x => x.Parent != null && x.Parent.Id == mnuItem.Id).OrderBy(c => c.Order))
                    {
                        strChilds += BuildFooterMenuItemMobile(m, list, ref level);
                        level--;
                    }

                    str = string.Format("<a href=\"{0}\" target=\"{1}\">{2}</a><span></span><ul>{3}</ul>",
                        !string.IsNullOrEmpty(mnuItem.Link) ? mnuItem.Link : "javascript:void(0)",
                        linkTarget,
                        mnuItem.Title,
                        strChilds);
                    str = string.Format("<li>{0}</li>", str);
                }
                else
                {
                    str = string.Format("<li><a href=\"{0}\" target=\"{1}\">{2}</a></li>",
                        !string.IsNullOrEmpty(mnuItem.Link) ? mnuItem.Link : "javascript:void(0)",
                        linkTarget, mnuItem.Title);
                }
            }

            return str;
        }
        #endregion
    }
}
