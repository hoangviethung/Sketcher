using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Article
{
    public class ArticleManageViewModel 
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng tên bài viết!")]
        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đường dẫn cho bài viết!")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string SeName { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public string ImageDefault { get; set; }

        public string ImageFolder { get; set; }

        public bool IsPublished { get; set; }
		
		public bool IsHot { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ!")]
        public long CategorySelectedValue { get; set; }
        public List<SelectListItem> Categories { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ!")]
        public int LanguageSelectedValue { get; set; }
        public List<SelectListItem> Languages { get; set; }

        public long LogHistoryId { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string MetaTitle { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string MetaDescription { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string MetaKeywords { get; set; }

        //[RegularExpression(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$", ErrorMessage ="Chuỗi Url nhập vào không hợp lệ")]
        public string ExternalUrl { get; set; }
      
        public ArticleManageViewModel()
        {
        }

        public ArticleManageViewModel(Core.Article entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Description = entity.Description;
            Body = entity.Body;
            ImageDefault = entity.ImageDefault;
            ImageFolder = entity.ImageFolder;
            MetaDescription = entity.MetaDescription;
            MetaKeywords = entity.MetaKeywords;
            MetaTitle = entity.MetaTitle;
            ExternalUrl = entity.ExternalUrl;
            IsPublished = entity.IsPublished;
            IsHot = entity.IsHot;
            SeName = entity.SeName;
        }

        public static void ToEntity(ArticleManageViewModel model, ref Core.Article entity)
        {
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.Body = model.Body;
            entity.ImageDefault = model.ImageDefault;
            entity.ImageFolder = model.ImageFolder;
            entity.MetaDescription = model.MetaDescription;
            entity.MetaKeywords = model.MetaKeywords;
            entity.MetaTitle = model.MetaTitle;
            entity.ExternalUrl = model.ExternalUrl;
            entity.IsPublished = model.IsPublished;
            entity.SeName = model.SeName;
            entity.IsHot = model.IsHot;
        }

    }
}
