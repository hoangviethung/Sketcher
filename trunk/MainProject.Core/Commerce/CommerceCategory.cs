using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Core.Commerce
{
    public class CommerceCategory : Seo
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string SeName { get; set; }

        public int Order { get; set; }

        public string ImageDefault { get; set; }

        public bool IsActive { get; set; }

        public bool IsHot { get; set; }

        public virtual CommerceCategory Parent { get; set; }

        public virtual Category Category { get; set; }

        public string GetFamilyPath()
        {
            if (Parent == null) return Name;
            return String.Format("{0} >> {1}", Parent.GetFamilyPath(), Name);
        }

        public string GetPrefixUrl()
       {
            if (!string.IsNullOrEmpty(ExternalUrl))
            {
                return ExternalUrl;
            }
            else if (Parent == null)
            {
                return String.Format("{0}/{1}", Category.GetPrefixUrl(), SeName);
            }

            return String.Format("{0}/{1}", Parent.GetPrefixUrl(), SeName);
        }
    }
}
