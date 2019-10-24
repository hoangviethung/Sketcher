using System.ComponentModel.DataAnnotations;

namespace MainProject.Core
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        public string OfficeName { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string AddressMap { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public bool IsPublished { get; set; }

        public int Order { get; set; }

        public virtual Language Language { get; set; }

        public virtual Region Region { get; set; }
    }
}
