using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Region
{
    public class RegionSelectModel
    {
        public int CitySelectedValue { get; set; }
        // List region of city
        public List<SelectListItem> Cities { get; set; }


        // Selected district
        [Required(ErrorMessage = "Vui lòng chọn Quận - Huyện!")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Vui lòng chọn Quận - Huyện!")]
        public int DistrictSelectedValue { get; set; }

        public RegionSelectModel(List<SelectListItem> cities, int cityId, int districtId)
        {
            Cities = cities;
            CitySelectedValue = cityId;
            DistrictSelectedValue = districtId;
        }
    }
}
