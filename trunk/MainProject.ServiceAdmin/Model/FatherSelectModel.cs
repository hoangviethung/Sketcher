using System.Collections.Generic;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model
{
    public class FatherSelectModel
    {
        public long FatherSelectedValue { get; set; }

        public string FatherSelectedStringValue { get; set; }

        public List<SelectListItem> Fathers { get; set; }
    }
}