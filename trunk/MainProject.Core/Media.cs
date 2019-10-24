using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainProject.Core
{
	[Table("Medias")]
    public class Media
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public string AltImage { get; set; }

        public string ImageDefault { get; set; }

        public virtual Album Album { get; set; }
    }
}
