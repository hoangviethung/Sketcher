using System.ComponentModel.DataAnnotations;

namespace MainProject.Core.Commerce
{
    public class ProductPropertyRef
    {
        [Key]
        public long Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Property Property { get; set; }

        public string Value { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
    }
}
