using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MainProject.Core;

namespace MainProject.ServiceAdmin.Helper
{
    public static class LanguageHelper
    {
        public static List<SelectListItem> BindDropdown(IList<Language> languages,
            int selectedvalue, SelectListItem defaulValue = null)
        {
            var model = languages.Select(
                            d => new SelectListItem
                            {
                                Text = d.LanguageName,
                                Value = d.Id.ToString(),
                                Selected = selectedvalue == d.Id
                            }).ToList();
            if (defaulValue != null)
            {
                model.Insert(0, defaulValue);
            }
            return model;
        }
    }
}