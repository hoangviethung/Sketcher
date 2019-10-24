using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Branch
{
    public class BranchManageViewModel
    {
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        [Required(ErrorMessage = "Vui lòng nhập tên chi nhánh!")]
        public string OfficeName { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string AddressMap { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Address { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Description { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        [StringLength(50, ErrorMessage = "Không được vượt quá 50 ký tự!")]
        public string Phone { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Không được vượt quá 100 ký tự!")]
        public string Fax { get; set; }

        public bool IsPublished { get; set; }

        public int Order { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ!")]
        public int LanguageSelectedValue { get; set; }
        public List<SelectListItem> Languages { get; set; }

        public BranchManageViewModel() { }

        public BranchManageViewModel(Core.Branch entity)
        {
            OfficeName = entity.OfficeName;
            Description = entity.Description;
            AddressMap = entity.AddressMap;
            Lat = entity.Lat;
            Lng = entity.Lng;
            Address = entity.Address;
            Phone = entity.Phone;
            Fax = entity.Fax;
            Email = entity.Email;
            IsPublished = entity.IsPublished;
            Order = entity.Order;
        }

        public void ToEntity(ref Core.Branch entity, BranchManageViewModel model)
        {
            entity.OfficeName = model.OfficeName;
            entity.Description = model.Description;
            entity.Lat = model.Lat;
            entity.Lng = model.Lng;
            entity.Address = model.Address;
            entity.AddressMap = model.AddressMap;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.Fax = model.Fax;
            entity.IsPublished = model.IsPublished;
            entity.Order = model.Order;
        }
    }
}
