using MainProject.Core.Commerce;
using MainProject.Core.Enums;
using MainProject.Framework.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.CommerceProperty
{
    public class PropertyManageModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thuộc tính!")]
        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public UnitTypeCollection UnitTypeSelected { get; set; }

        public List<SelectListItem> UnitTypeItems { get; set; }

        public bool IsPriceEffect { get; set; }

        public PropertyManageModel() { }

        public PropertyManageModel(Property entity)
        {
            UnitTypeItems = EnumHelper.ToSelectList(typeof(UnitTypeCollection), entity?.UnitType);

            // Incase Update property
            if (entity != null)
            {
                Id = entity.Id;
                Name = entity.Name;
                UnitTypeSelected = entity.UnitType;
                IsPriceEffect = entity.IsPriceEffect;
            }
        }
    }
}

