using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MainProject.Core.Commerce;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model.CommerceCategory;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;

namespace MainProject.ServiceAdmin.Helper
{
    public static class CommerceCategoryHelper
    {
        #region selectedlist item
        /// <summary>
        /// Get commerce product category to select list
        /// </summary>
        /// <param name="commerceCategoryRepository"></param>
        /// <param name="selectedValue"></param>
        /// <param name="isContainDefault"></param>
        /// <returns></returns>
        public static List<SelectListItem> BindSelectListItem(CommerceCategoryRepository commerceCategoryRepository,
            long selectedValue,
            long exceptedId = 0,
            bool isContainDefault = false)
        {
            var model = new List<SelectListItem>();
            // Check whether contain default value
            if (isContainDefault)
            {
                model.Insert(0, new SelectListItem
                {
                    Selected = selectedValue == 0 ? true : false,
                    Text = "Danh mục gốc",
                    Value = "0"
                });
            }
            // Get all commerce product category
            var categories = commerceCategoryRepository.Find(c => c.Parent == null).ToList();

            // Loop to bind select list
            foreach (var category in categories)
            {
                // Avoid category reference to itself
                if (exceptedId == category.Id) continue;
                var selectListItem = new SelectListItem
                {
                    Selected = category.Id == selectedValue,
                    Text = string.Format("{0}", category.Name),
                    Value = category.Id.ToString()
                };
                // Add item to model
                model.Add(selectListItem);
                // Bind child commerce product category
                model.AddRange(BuildSelectListItemsOfChild(commerceCategoryRepository, selectedValue, category.Id,
                                                           selectListItem.Text, exceptedId));
            }

            return model;
        }

        /// <summary>
        /// Get child commerce product category to model
        /// </summary>
        /// <param name="commerceCategoryRepository"></param>
        /// <param name="selectedValue">Check whether category is selected</param>
        /// <param name="categoryId">Parent category id</param>
        /// <param name="prefix">Url is binded</param>
        /// <returns></returns>
        private static IList<SelectListItem> BuildSelectListItemsOfChild(CommerceCategoryRepository commerceCategoryRepository,
            long selectedValue, long categoryId, string prefix, long exceptedId = 0)
        {
            var listResult = new List<SelectListItem>();
            foreach (var item in commerceCategoryRepository.Find(c => c.Parent.Id == categoryId).ToList())
            {
                // Avoid category reference to itself
                if (exceptedId == item.Id) continue;
                var selectListItem = new SelectListItem
                {
                    Selected = item.Id == selectedValue,
                    Text = prefix + " >> " + item.Name,
                    Value = item.Id.ToString()
                };

                listResult.Add(selectListItem);
                listResult.AddRange(BuildSelectListItemsOfChild(commerceCategoryRepository, selectedValue, item.Id, selectListItem.Text, exceptedId));
            }

            return listResult;
        }
        #endregion

        #region update entity

        public static BaseResponseModel CreateOrUpdate(CommerceCategoryRepository commerceCategoryRepository,
            CommerceCategoryManageModel model, LogHistoryService logHistoryService, CategoryRepository categoryRepository)
        {
            CommerceCategory entity = null;
            // Check wheter Create
            if (model.Id == 0)
            {
                entity = new CommerceCategory() {
                    Parent = commerceCategoryRepository.FindUnique(c => c.Id == model.ParentSelectedValue),
                    Category = categoryRepository.FindUnique(x => x.DisplayTemplate == DisplayTemplateCollection.Product)
                };
                CommerceCategoryManageModel.ToEntity(model, ref entity);
                // Insert data into db
                commerceCategoryRepository.Insert(entity);
                entity.SeName = SeoHelper.ValidateSeNameAndSubmit(EntityTypeCollection.CommerceCategory, entity.Id, entity.SeName, entity.Name, null);
                commerceCategoryRepository.SaveChanges();
                //save logHistory
                logHistoryService.Update(model.LogHistoryId, entity.Id);
            }
            else // Edit
            {
                // Get commerce category by id
                entity = commerceCategoryRepository.FindUnique(c => c.Id == model.Id);
                if (entity == null)
                {
                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.BadRequest,
                        Message = string.Format("<strong style='color:green'>Đối tượng không tồn tại!</strong>")
                    };
                }

                // Parse model to entity
                CommerceCategoryManageModel.ToEntity(model, ref entity);
                entity.Parent = commerceCategoryRepository.FindUnique(c => c.Id == model.ParentSelectedValue);
                // Save commerce category
                commerceCategoryRepository.SaveChanges();
                entity.SeName = SeoHelper.ValidateSeNameAndSubmit(
                                        EntityTypeCollection.CommerceCategory, entity.Id, entity.SeName, entity.Name, null);
                commerceCategoryRepository.SaveChanges();

                //save logHistory
                logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });
            }

            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Thêm thành công!</strong>")
            };
        }

        #endregion
    }
}