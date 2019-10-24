using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class StringResourceValue
    {
        [Key]
        public long Id { get; set; }

        public virtual StringResourceKey Key { get; set; }

        public virtual Language Language { get; set; }

        public string Value { get; set; }
    }
}
