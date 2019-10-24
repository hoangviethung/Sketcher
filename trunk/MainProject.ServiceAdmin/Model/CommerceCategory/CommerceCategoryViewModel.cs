using System.Collections.Generic;

namespace MainProject.ServiceAdmin.Model.CommerceCategory
{
    public class CommerceCategoryModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ParentPath { get; set; }

        public int Order { get; set; }

        public string Url { get; set; }

        public CommerceCategoryModel(Core.Commerce.CommerceCategory entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ParentPath = entity.Parent != null ? entity.Parent.GetFamilyPath() : string.Empty;
            Order = entity.Order;
            Url = entity.GetPrefixUrl();
        }
    }
}
