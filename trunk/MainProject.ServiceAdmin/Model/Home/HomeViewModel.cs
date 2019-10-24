using MainProject.Core;
using System.Collections.Generic;

namespace MainProject.ServiceAdmin.Model.Home
{
    public class HomeViewModel
    {
    }

    public class ProductPurchased
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public long Id { get; set; }
        public List<LanguageUrlModel> LanguageUrlModel { get; set; }
    }

    public class LanguageUrlModel
    {
        public string Url { get; set; }

        public Language Language { get; set; }
    }

    public class StatisticaTotal
    {
        public string toDay { get; set; }
        public string thisWeek { get; set; }
        public string thisMonth { get; set; }
        public string thisYear { get; set; }
        public string allTime { get; set; }
    }
}
