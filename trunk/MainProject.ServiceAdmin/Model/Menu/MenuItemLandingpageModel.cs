using MainProject.Core;

namespace MainProject.ServiceAdmin.Model.Menu
{
    public class MenuItemModel
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public string Title { get; set; }

        public string ParentPath { get; set; }

        public string Link { get; set; }

        public Language Language { get; set; }

        public MenuItemModel(MenuItem entity)
        {
            Id = entity.Id; 
            Link = entity.Link; 
            Title = entity.Title; 
            Order = entity.Order;
            ParentPath = entity.GetParent(); 
            Language = entity.Language;
        }
    }
}
