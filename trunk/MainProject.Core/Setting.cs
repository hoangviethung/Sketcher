using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
