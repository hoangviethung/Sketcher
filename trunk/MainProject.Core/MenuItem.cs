using System;
using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class MenuItem
    {
        public MenuItem()
        {
            Order = 1;
        }

        [Key]
        public long Id { get; set; }

        public string Title { get; set; }
        
        public int Order { get; set; }

        public string Link { get; set; }

        public LinkTargetCollection LinkTarget { get; set; }

        public virtual MenuItem Parent { get; set; }

        public virtual Language Language { get; set; }

        public virtual Menu Menu { get; set; }

        public string ImageUrl { get; set; }

        public string GetParent()
        {
            if (Parent == null) return "";
            if (Parent.Parent == null) return Parent.Title;
            return String.Format("{0} >> {1}", Parent.GetParent(), Parent.Title);
        }
    }
}
