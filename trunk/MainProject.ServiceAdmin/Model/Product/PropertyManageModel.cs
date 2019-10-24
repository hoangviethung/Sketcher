using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Product
{
    public class PropertyManageModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "Không được vượt quá 50 ký tự!")]
        public string Value { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public int Position { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Vui lòng chọn thuộc tính!")]
        public int PropertySelectedId { get; set; }
        public List<SelectListItem> Properties { get; set; }

        public PropertyManageModel() { }

        public PropertyManageModel(Core.Commerce.ProductPropertyRef entity)
        {
            Id = entity.Id;
            Value = entity.Value;
            Price = entity.Price;
            PropertySelectedId = entity.Property.Id;
            Properties = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = entity.Property.Name,
                    Value = entity.Property.Id.ToString(),
                }
            };
        }
    }
}
