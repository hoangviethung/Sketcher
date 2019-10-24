using System.Collections.Generic;
using MainProject.Core;
using MainProject.Framework.Models;

namespace MainProject.Service.Model.FAQ
{
    public class FaqItemsViewModel
    {
        public List<FaqItem> ListFaqItem { get; set; }
        public PagingModel PagingViewModel { get; set; }

        //Added by MK
        public Category Category { get; set; }
    }
}