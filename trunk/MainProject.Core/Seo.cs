using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Seo
    {
        [StringLength(500)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string MetaDescription { get; set; }

        [StringLength(500)]
        public string MetaKeywords { get; set; }

        public string ExternalUrl { get; set; }
    }
}
