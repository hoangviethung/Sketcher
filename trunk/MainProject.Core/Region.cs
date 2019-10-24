using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class Region
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập tọa độ Lng")]
        public double Lng { get; set; }
        [Required(ErrorMessage = "Nhập tọa độ Lat")]
        public double Lat { get; set; }

        [Required(ErrorMessage = "Nhập tên")]
        [StringLength(500)]
        public string Name { get; set; }

        public string ZipCode { get; set; }

        public decimal? FeeShip { get; set; }

        public virtual Region Parent { get; set; }

        public RegionTypeCollection RegionType { get; set; }
    }
}
