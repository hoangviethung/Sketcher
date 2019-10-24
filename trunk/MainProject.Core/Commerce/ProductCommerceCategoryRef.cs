using System.ComponentModel.DataAnnotations;

namespace MainProject.Core.Commerce
{
    public class ProductCommerceCategoryRef
    {
        [Key]
        public long Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual CommerceCategory CommerceCategory { get; set; }
    }
}
