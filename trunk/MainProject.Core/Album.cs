using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Album
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public string ResourcePath { get; set; }

        public string ImageFolder { get; set; }

        public bool IsVideo { get; set; }

        public bool IsPublished { get; set; }

        public virtual Language Language { get; set; }
    }
}
