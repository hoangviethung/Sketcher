using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model.Album
{
    public class MediaManageViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hình!")]
        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public int Position { get; set; }

        public int Order { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string AltImage { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đường dẫn ảnh!")]
        public string ImageDefault { get; set; }

        public Core.Album Album { get; set; }

        public MediaManageViewModel() { }

        public MediaManageViewModel(Core.Media entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Order = entity.Order;
            AltImage = entity.AltImage;
            ImageDefault = entity.ImageDefault;
            IsDeleted = false;
        }

        public void ToEntity(MediaManageViewModel model, ref Core.Media entity)
        {
            entity.Name = model.Name;
            entity.Order = model.Order;
            entity.AltImage = model.AltImage;
            entity.ImageDefault = model.ImageDefault;
        }
    }
}
