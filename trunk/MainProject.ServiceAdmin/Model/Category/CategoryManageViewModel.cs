using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MainProject.Core.Enums;

namespace MainProject.ServiceAdmin.Model.Category
{
    public class CategoryManageViewModel
    {
        public long Id { get; set; }

        [StringLength(255, ErrorMessage = "Không được vượt quá 255 ký tự!")]
        [Required(ErrorMessage = "Vui lòng điền tên danh mục!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng điền đường dẫn cho danh mục!")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string SeName { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Vui lòng chỉ nhập số!")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục!")]
        public int ParentSelectedValue { get; set; }
        public List<SelectListItem> Parents { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ!")]
        public int LanguageSelectedValue { get; set; }
        public List<SelectListItem> Languages { get; set; }

        public DisplayTemplateCollection DisplayTemplateSelected { get; set; }
        public List<SelectListItem> DisplayTemplates { get; set; }

        public string ImageDefault { get; set; }

        public string ImageFolder { get; set; }

        public bool PrivateArea { get; set; }

        public long LogHistoryId { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string MetaTitle { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string MetaDescription { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string MetaKeywords { get; set; }
        
        public string ExternalUrl { get; set; }

        public string PreviewLink { get; set; }

        public CategoryManageViewModel() { }

        public CategoryManageViewModel(Core.Category entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Order = entity.Order;
            PrivateArea = entity.PrivateArea;
            LanguageSelectedValue = Convert.ToInt32(entity.Language.Id);
            ParentSelectedValue = Convert.ToInt32(entity.Parent.Id);
            MetaTitle = entity.MetaTitle;
            MetaDescription = entity.MetaDescription;
            MetaKeywords = entity.MetaKeywords;
            DisplayTemplateSelected = entity.DisplayTemplate;
            ImageDefault = entity.ImageDefault;
            ImageFolder = entity.ImageFolder;  
            ExternalUrl = entity.ExternalUrl;
            SeName = entity.SeName;
            PreviewLink = entity.GetPrefixUrl();
        }

        public static void ToEntity(CategoryManageViewModel model, ref Core.Category entity)
        {
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.Order = model.Order;
            entity.PrivateArea = model.PrivateArea;
            entity.MetaTitle = model.MetaTitle;
            entity.MetaDescription = model.MetaDescription;
            entity.MetaKeywords = model.MetaKeywords;
            entity.ImageDefault = model.ImageDefault;
            entity.ImageFolder = model.ImageFolder;
            entity.DisplayTemplate = model.DisplayTemplateSelected;
            entity.ExternalUrl = model.ExternalUrl;
            entity.SeName = model.SeName;
        }
    }
}