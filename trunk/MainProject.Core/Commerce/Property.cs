using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core.Commerce
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public UnitTypeCollection UnitType { get; set; }

        public bool IsPriceEffect { get; set; }
    }
}
