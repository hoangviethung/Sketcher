using MainProject.Core;
using MainProject.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Helper
{
    public class RegionHelper
    {
        public static List<SelectListItem> BindCitySelectListItem(List<Region> regions, long selectedValue)
            => regions.Select(c => new SelectListItem()
                      {
                          Text = c.Name,
                          Value = c.Id.ToString(),
                          Selected = c.Id == selectedValue
                      }).ToList();

        public static List<SelectListItem> BindDistrictSelectListItem(List<Region> regions, long cityId, long selectedValue)
            => regions.Where(c => c.RegionType == RegionTypeCollection.District && c.Parent.Id == cityId)
                      .Select(c => new SelectListItem() {
                          Text = c.Name,
                          Value = c.Id.ToString(),
                          Selected = c.Id == selectedValue
                      }).ToList();
    }
}