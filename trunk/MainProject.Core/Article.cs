using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Article:Seo
    {
        [Key]
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string SeName { get; set; }

        public string ImageDefault { get; set; }

        public string ImageFolder { get; set; }

        public int ViewCount { get; set; }

        public virtual Category Category { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public bool IsPublished { get; set; }

        public virtual Language Language { get; set; }

        public bool IsHot { get; set; }

        public int Order { get; set; }

        public string GetUrl()
        {
            if (Category == null)
            {
                return string.Format("/{0}", SeName);
            }
            else
            {
                return string.Format("{0}/{1}", Category.GetPrefixUrl(), SeName);
            }            
        }
    }
}
