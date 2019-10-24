using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model
{
    public class ImageManageModel
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

        public string Video { get; set; }

        [StringLength(50, ErrorMessage = "Không được vượt quá 50 ký tự!")]
        public Core.Category Category { get; set; }

        public ImageManageModel() { }

        public ImageManageModel(Core.Image entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Order = entity.Order;
            AltImage = entity.AltImage;
            ImageDefault = entity.ImageDefault;
            Video = entity.Video;
            IsDeleted = false;
        }

        public void ToEntity(ImageManageModel model, ref Core.Image entity)
        {
            entity.Name = model.Name;
            entity.Order = model.Order;
            entity.AltImage = model.AltImage;
            entity.ImageDefault = model.ImageDefault;
            entity.Video = model.Video;
        }
    }
}
