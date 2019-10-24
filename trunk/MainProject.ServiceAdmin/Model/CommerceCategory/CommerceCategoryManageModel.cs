using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.CommerceCategory
{
    public class CommerceCategoryManageModel
    {
        public long LogHistoryId { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục!")]
        [StringLength(200, ErrorMessage = "Độ dài không được vượt quá 200 ký tự!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Vui lòng điền đường dẫn cho danh mục!")]
        [StringLength(500, ErrorMessage = "Độ dài không được vượt quá 500 ký tự!")]
        public string SeName { get; set; }

        public long ParentSelectedValue { get; set; }
        public List<SelectListItem> Parents { get; set; }


        [StringLength(500, ErrorMessage = "Độ dài không được vượt quá 500 ký tự!")]
        public string ImagePath { get; set; }

        public bool IsActive { get; set; }

        public bool IsHot { get; set; }

        public int Order { get; set; }

        [StringLength(500)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string MetaDescription { get; set; }

        [StringLength(500)]
        public string MetaKeywords { get; set; }

        public CommerceCategoryManageModel()
        {
        }

        public CommerceCategoryManageModel(Core.Commerce.CommerceCategory entity)
        {
            Id = entity.Id;
            SeName = entity.SeName;
            Name = entity.Name;
            ParentSelectedValue = entity.Parent != null ? entity.Parent.Id : 0;
            Order = entity.Order;
            ImagePath = entity.ImageDefault;
            IsActive = entity.IsActive;
            IsHot = entity.IsHot;
        }

        public static void ToEntity (CommerceCategoryManageModel model, ref Core.Commerce.CommerceCategory entity)
        {
            entity.SeName = model.SeName;
            entity.Name = model.Name;
            entity.Order = model.Order;
            entity.ImageDefault = model.ImagePath;
            entity.IsActive = model.IsActive;
            entity.IsHot = model.IsHot;
            entity.MetaTitle = model.MetaTitle;
            entity.MetaKeywords = model.MetaKeywords;
            entity.MetaDescription = model.MetaDescription;
        }

        /// <summary>
        /// Bind entity to model for Create and Edit View
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="_commerceCategoryRepository"></param>
        /// <returns></returns>
        public static CommerceCategoryManageModel InitManageModel(Core.Commerce.CommerceCategory entity)
        {
            var model = new CommerceCategoryManageModel
            {
                Id = 0
            };
            // Check whether entity is exist (Edit)
            if (entity != null)
            {
                model = new CommerceCategoryManageModel(entity);
            }

            return model;
        }
    }
}
