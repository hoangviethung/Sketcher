using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class Menu
    {
        [Key]
        public long Id { get; set; }

        public string CodeName { get; set; }

        public MenuTypeCollection Type { get; set; }

        public int Order { get; set; }
    }
}
