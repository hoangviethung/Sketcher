using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Region
{
    public class CityManageViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string Name { get; set; }

        public CityManageViewModel() { }

        public CityManageViewModel(Core.Region entity)
        {
            Name = entity.Name;
        }

        public static void ToEntity(CityManageViewModel model, ref Core.Region entity)
        {
            entity.Name = model.Name;
        }
    }

    public class DisctrictManageViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string Name { get; set; }

        public int CitySelectedValue { get; set; }
        public List<SelectListItem> Cities { get; set; }

        public DisctrictManageViewModel() { }

        public DisctrictManageViewModel(Core.Region entity)
        {
            Name = entity.Name;
            CitySelectedValue = entity.Parent.Id;
        }

        public static void ToEntity(DisctrictManageViewModel model, ref Core.Region entity)
        {
            entity.Name = model.Name;
        }
    }
}
