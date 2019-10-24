using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Album
{
    public class GalleryManageViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề Album!")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string Name { get; set; }

        public int Order { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đường dẫn ảnh mặc định!")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string ResourcePath { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string ImageFolder { get; set; }

        public bool IsVideo { get; set; }

        public bool IsPublished { get; set; }

        [Required]
        public int LanguageSelectedValue { get; set; }
        public List<SelectListItem> Languages { get; set; }

        public List<MediaManageViewModel> Medias { get; set; }

        public long LogHistoryId { get; set; }

        public GalleryManageViewModel() { }

        public GalleryManageViewModel(Core.Album entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Order = entity.Order;
            ResourcePath = entity.ResourcePath;
            ImageFolder = entity.ImageFolder;
            IsPublished = entity.IsPublished;
            IsVideo = entity.IsVideo;
            LanguageSelectedValue = entity.Language.Id;
        }

        public void ToEntity(GalleryManageViewModel model, ref Core.Album entity)
        {
            entity.Id = model.Id;
            entity.Name = model.Name;
            entity.Order = model.Order;
            entity.ResourcePath = model.ResourcePath;
            entity.ImageFolder = model.ImageFolder;
            entity.IsPublished = model.IsPublished;
            entity.IsVideo = model.IsVideo;
        }
    }
}
