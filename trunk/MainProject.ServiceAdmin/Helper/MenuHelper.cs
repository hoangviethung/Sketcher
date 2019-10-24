using MainProject.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Helper
{
   public class MenuHelper
    {
        public static List<SelectListItem> BindDropdown(IList<Menu> menus, long selectedvalue)
        {
            return menus.Select(
                d => new SelectListItem
                {
                    Text = d.CodeName,
                    Value = d.Id.ToString(),
                    Selected = selectedvalue == d.Id
                }).ToList();
        }
    }
}
