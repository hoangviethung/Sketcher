using MainProject.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ServiceAdmin.Model.Role
{
   public class FeatureGroupViewModel
    {
        public FeatureGroup FeatureGroup { get; set; }

        public string FeatureGroupName { get; set; }

        public List<FeatureViewModel> Features { get; set; }
    }
}
