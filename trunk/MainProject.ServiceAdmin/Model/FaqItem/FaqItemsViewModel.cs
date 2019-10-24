using MainProject.Framework.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.FaqItem
{
   public class FaqItemsViewModel
    {
        public List<Core.FaqItem> ListFaqItem { get; set; }
        public PagingModel PagingViewModel { get; set; }

        public long FaqCategorySelectedValue { get; set; }
        public List<SelectListItem> FaqCategories { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngôn ngữ!")]
        public int LanguageSelectedValue { get; set; }
        public List<SelectListItem> Languages { get; set; }

        public int StatusSelectedValue { get; set; }
        public List<SelectListItem> ApprovalStatusList { get; set; }

        public string Title { get; set; }
    }
}
