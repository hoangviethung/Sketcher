using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Product
{
    public class ProductManageModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên chung của sản phẩm!")]
        [StringLength(200, ErrorMessage = "Không được nhập quá 200 ký tự!")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Không được nhập quá 100 ký tự!")]
        public string ProductCode { get; set; }

        public string SeName { get; set; }

        public bool IsPromotion { get; set; }

        [Range(10, int.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn 10 VND")]
        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

        [StringLength(500, ErrorMessage = "Không được nhập quá 500 ký tự!")]
        public string ImageDefault { get; set; }

        public string ImageFolder { get; set; }  

        public string Description { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        [AllowHtml]
        public string Specification { get; set; }

        public List<long> SelectedCommerceCategoryIds { get; set; }
        public List<SelectListItem> CommerceCategories { get; set; }

        public List<PropertyManageModel> Properties { get; set; }

        public int Order { get; set; }

        public bool IsNew { get; set; }

        public bool IsHot { get; set; }

        public long LogHistoryId { get; set; }

        #region SEO
        [StringLength(500)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string MetaDescription { get; set; }

        [StringLength(500)]
        public string MetaKeywords { get; set; }

        public string ExternalUrl { get; set; }
        #endregion

        #region Shipping
        [Range(1, 100000, ErrorMessage = "Cân nặng phải lớn hơn 0 và nhỏ hơn 100000 gram")]
        public int Weight { get; set; }

        [Range(1, 200, ErrorMessage = "Chiều dài phải lớn hơn 0 và nhỏ hơn 200 cm")]
        public int Length { get; set; }

        [Range(1, 200, ErrorMessage = "Chiều rộng phải lớn hơn 0 và nhỏ hơn 200 cm")]
        public int Width { get; set; }

        [Range(1, 200, ErrorMessage = "Chiều cao phải lớn hơn 0 và nhỏ hơn 200 cm")]
        public int Height { get; set; }
        #endregion

        public ProductManageModel() { }

        public ProductManageModel(Core.Commerce.Product entity)
        {
            Id = entity.Id;
            SeName = entity.SeName;
            ImageDefault = entity.ImageDefault;
            ImageFolder = entity.ImageFolder;
            Name = entity.Name;
            ProductCode = entity.ProductCode;
            IsPromotion = entity.IsPromotion;
            Description = entity.Description;
            Specification = entity.Specification;
            Body = entity.Body;
            IsHot = entity.IsHot;
            IsNew = entity.IsNew;
            Price = entity.Price;
            PromotionPrice = entity.PromotionPrice;
            MetaTitle = entity.MetaTitle;
            MetaKeywords = entity.MetaKeywords;
            MetaDescription = entity.MetaDescription;
            Weight = entity.Weight;
            Length = entity.Length;
            Width = entity.Width;
            Height = entity.Height;
        }

        public static void ToEntity(ProductManageModel model, ref Core.Commerce.Product entity)
        {
            entity.Name = model.Name;
            entity.SeName = model.SeName;
            entity.ProductCode = model.ProductCode;
            entity.IsPromotion = model.IsPromotion;
            entity.ImageDefault = model.ImageDefault;
            entity.ImageFolder = model.ImageFolder;
            entity.Description = model.Description;
            entity.Specification = model.Specification;
            entity.Body = model.Body;
            entity.IsHot = model.IsHot;
            entity.IsNew = model.IsNew;
            entity.Price = model.Price;
            entity.PromotionPrice = model.PromotionPrice;
            entity.MetaTitle = model.MetaTitle;
            entity.MetaKeywords = model.MetaKeywords;
            entity.MetaDescription = model.MetaDescription;
            entity.Weight = model.Weight;
            entity.Length = model.Length;
            entity.Width = model.Width;
            entity.Height = model.Height;
            // Onepay minumum price for transaction
            if (model.IsPromotion && model.PromotionPrice < 10)
            {
                model.PromotionPrice = model.Price;
            }
        }
    }
}
