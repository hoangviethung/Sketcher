using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Image
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public string AltImage { get; set; }

        public string ImageDefault { get; set; }

        public string Video { get; set; }

        public virtual Article Article { get; set; }
    }
}
