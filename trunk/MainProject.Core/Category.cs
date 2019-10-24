using System;
using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class Category : Seo
    {
        public Category()
        {
            Order = 1;
        }

        [Key]
        public long Id { get; set; }

        public string Title { get; set; }

        public string SeName { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }

        public int Order { get; set; }

        public virtual Category Parent { get; set; }

        public virtual Language Language { get; set; }

        public DisplayTemplateCollection DisplayTemplate { get; set; }

        public EntityTypeCollection EntityType { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool PrivateArea { get; set; }

        public string ImageDefault { get; set; }

        public string ImageFolder { get; set; }

        public string GetPrefixUrl()
        {
			if (!string.IsNullOrEmpty(ExternalUrl)) return ExternalUrl;
			
            if (Parent == null) return IsSystem ? "" : "/" + SeName;
            
            return String.Format("{0}/{1}", Parent.GetPrefixUrl(), SeName);
        }
    }
}
