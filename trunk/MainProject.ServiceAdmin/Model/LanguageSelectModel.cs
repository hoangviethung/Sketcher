using MainProject.Framework.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model
{
    public class LanguageSelectModel
    {
        public int LanguageSelectedValue { get; set; }

        public List<SelectListItem> Languages { get; set; }
    }
}