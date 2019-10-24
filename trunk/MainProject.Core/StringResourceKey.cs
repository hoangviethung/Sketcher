using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class StringResourceKey
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
