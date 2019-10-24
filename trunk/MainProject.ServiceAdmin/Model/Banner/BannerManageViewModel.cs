using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Banner
{
    public class BannerManageViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Nhập tên")]
        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Nhập hình ảnh")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string ResourcePath { get; set; }

        public string Description { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string Link { get; set; }

        public int Order { get; set; }

        public bool IsPublished { get; set; }

        public string ImageFolder { get; set; }

        public long LogHistoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ!")]
        public int LanguageSelectedValue { get; set; }
        public List<SelectListItem> Languages { get; set; }

        public BannerManageViewModel() {}

        public BannerManageViewModel(Core.Banner entity)
        {
            Name = entity.Name;
            Description = entity.Description;
            ImageFolder = entity.ImageFolder;
            ResourcePath = entity.ResourcePath;
            IsPublished = entity.IsPublished;
            Order = entity.Order;
            Link = entity.Link;
        }
        public void ToEntity(BannerManageViewModel model, ref Core.Banner entity)
        {
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.ImageFolder = model.ImageFolder;
            entity.ResourcePath = model.ResourcePath;
            entity.IsPublished = model.IsPublished;
            entity.Order = model.Order;
            entity.Link = model.Link;
        }
    }
}
