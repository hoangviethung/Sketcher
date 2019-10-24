using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;

namespace MainProject.Web.Areas.Admin.Helpers
{
    public static class CategoryHelper
    {
        #region selectedlist item for create, edit
        /// <summary>
        /// Get all category
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="selectedValue">For display item select in html drop down box</param>
        /// <param name="languageValue">Language id</param>
        /// <param name="isIncludeRootCategory">Get root item</param>
        /// <param name="exceptedId">Category don't get</param>
        /// <param name="displayTemplateCollections">Filter by display template</param>
        /// <returns></returns>
        public static List<SelectListItem> BindSelectListItem(CategoryRepository categoryRepository,
            long selectedValue,
            int languageValue,
            long exceptedId = 0,
            bool isIncludeRootCategory = true,
            FilterGroup filterGroup = FilterGroup.None
            )
        {
            // Get root Id
            long rootId = ConfigItemHelper.GetRootNewsId();
            // Initialize model
            var model = new List<SelectListItem>();
            // Initialize displayTemplate
            List<DisplayTemplateCollection> displayTemplateCollections = null;
            if (filterGroup != FilterGroup.None)
            {
                displayTemplateCollections = EnumHelper.GetFilterTemplates(filterGroup);
            }
            // Check whether get root category
            if (isIncludeRootCategory)
            {
                model.Add(new SelectListItem
                {
                    Value = rootId.ToString(),
                    Text = "Danh mục gốc!",
                    Selected = rootId == selectedValue
                });
            }
            // Get all category has parent is root
            var categories = displayTemplateCollections == null
                           ? categoryRepository.Find(c => c.Parent.Id == rootId && c.Language.Id == languageValue).ToList()
                           : categoryRepository.Find(c => c.Parent.Id == rootId
                                                       && c.Language.Id == languageValue
                                                       && displayTemplateCollections.Contains(c.DisplayTemplate)).ToList();
            // Loop categories for getting own child category
            foreach (var category in categories)
            {
                // Avoid category reference to itself
                if (exceptedId == category.Id) continue;
                // Bind data to model
                var selectListItem = new SelectListItem
                {
                    Selected = category.Id == selectedValue,
                    Text = category.Title,
                    Value = category.Id.ToString()
                };
                // Don't get parent
                if (category.DisplayTemplate.IsExcept(filterGroup))
                {
                    // Add model to list item
                    model.Add(selectListItem);
                }
                // Add child category model to list item
                model.AddRange(BuildSelectListItemsOfChild(categoryRepository, selectedValue, category.Id,
                                                           selectListItem.Text, exceptedId, filterGroup));
            }

            return model;
        }
        #endregion

        #region eleclist item of child category
        /// <summary>
        /// Get child categories of an category
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="selectedValue">For display item select in html drop down box</param>
        /// <param name="categoryId">Parent category id</param>
        /// <param name="prefix">Parent category title is customized of category</param>
        /// <param name="exceptedId">Category don't get</param>
        /// <param name="displayTemplateCollections">Filter by display template</param>
        /// <returns></returns>
        private static IList<SelectListItem> BuildSelectListItemsOfChild(CategoryRepository categoryRepository,
            long selectedValue,
            long categoryId,
            string prefix,
            long exceptedId = 0,
            FilterGroup filterGroup = FilterGroup.None)
        {
            var listResult = new List<SelectListItem>();
            // Initialize displayTemplate
            List<DisplayTemplateCollection> displayTemplateCollections = null;
            if (filterGroup != FilterGroup.None)
            {
                displayTemplateCollections = EnumHelper.GetFilterTemplates(filterGroup);
            }
            // Get all category has parent is root
            var categories = displayTemplateCollections == null
                                        ? categoryRepository.Find(c => c.Parent.Id == categoryId).ToList()
                                        : categoryRepository.Find(c => c.Parent.Id == categoryId
                                                                && displayTemplateCollections.Contains(c.DisplayTemplate)).ToList();

            // Loop child categories for binding data to model
            foreach (var item in categories)
            {
                // Avoid category reference to itself
                if (exceptedId == item.Id) continue;
                // Bind data to model
                var selectListItem = new SelectListItem
                {
                    Selected = item.Id == selectedValue,
                    Text = prefix + " >> " + item.Title,
                    Value = item.Id.ToString()
                };
                // Don't get parent
                if (item.DisplayTemplate.IsExcept(filterGroup))
                {
                    // Add model to list item
                    listResult.Add(selectListItem);
                }
                // Add child category model to list item
                listResult.AddRange(BuildSelectListItemsOfChild(categoryRepository, selectedValue, item.Id,
                                                                selectListItem.Text, exceptedId, filterGroup));
            }

            return listResult;
        }
        #endregion
    }
}