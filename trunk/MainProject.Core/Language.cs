using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        public string LanguageName { get; set; }

        public string LanguageKey { get; set; }

        public string Image { get; set; }

        public bool IsDefault { get; set; }
    }
}
