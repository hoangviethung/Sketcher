using System;
using MainProject.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Banner
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsPublished { get; set; }

        public virtual Language Language { get; set; }

        public bool IsVideo { get; set; }

        public string Link { get; set; }

        public string AltImage { get; set; }

        public string ResourcePath { get; set; }

        public int Order { get; set; }

        public string ImageFolder { get; set; }
    }
}
