using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Core.Commerce
{
    public class CommerceCategoryTranslation
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual Language Language { get; set; }

        public virtual CommerceCategory CommerceCategory { get; set; }

        public string SeName { get; set; }
    }
}
