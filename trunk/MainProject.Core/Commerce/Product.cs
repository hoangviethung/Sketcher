using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MainProject.Core.Commerce
{
    public class Product : Seo
    {
        [Key]
        public long Id { get; set; }

        public string ImageDefault { get; set; }

        public string ImageFolder { get; set; }

        public string SeName { get; set; }

        public string Name { get; set; }

        public string ProductCode { get; set; }

        public decimal Price { get; set; }

        public int Weight { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool IsPromotion { get; set; }

        public decimal PromotionPrice { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public string Body { get; set; }
        
        public int Order { get; set; }

        public bool IsHot { get; set; }

        public bool IsNew { get; set; }

        public bool IsLocked { get; set; }

        public bool IsHide { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<ProductCommerceCategoryRef> ProductCommerceCategoryRefs { get; set; }

        public decimal GetPrice()
        {
            return IsPromotion ? PromotionPrice : Price;
        }

        public string GetUrl()
        {
            if (!string.IsNullOrEmpty(ExternalUrl))
            {
                return ExternalUrl;
            }

            return string.Format("{0}/{1}", ProductCommerceCategoryRefs.First().CommerceCategory.GetPrefixUrl(), SeName);
        }
    }
}
