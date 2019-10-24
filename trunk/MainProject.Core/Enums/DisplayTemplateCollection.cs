using System;
using System.ComponentModel;

namespace MainProject.Core.Enums
{
    public enum DisplayTemplateCollection
    {
        // For SEO Home Page
        [Description("Trang chủ")]
        Home = 1,
        
        [Description("Trang sản phẩm")]
        Product = 2,
    }

    public enum FilterGroup
    {
        None = 0,
    }

    /// <summary>
    /// Filter category for article admin
    /// </summary>
    public class TemplateAttribute : Attribute
    {
        public TemplateAttribute() { }

        public TemplateAttribute(FilterGroup group, bool isExcept = false)
        {
            Group = group;
            IsExcept = isExcept;
        }

        public FilterGroup Group { get; set; }

        public bool IsExcept { get; set; }
    }
}
