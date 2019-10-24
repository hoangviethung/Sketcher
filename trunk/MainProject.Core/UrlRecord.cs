using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class UrlRecord
    {
        [Key]
        public long Id { get; set; }

        public virtual Language Language { get; set; }

        public string SeName { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string OriginUrl { get; set; }

        public long EntityId { get; set; }

        public EntityTypeCollection EntityType { get; set; }

        public bool IsDisabled { get; set; }
    }
}
