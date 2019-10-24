using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ServiceAdmin.Model.Role
{
   public class FeatureViewModel
    {
        public int FeatureValue { get; set; }

        public string FeatureName { get; set; }

        public bool HasViewPermission { get; set; }

        public bool HasCreatePermission { get; set; }

        public bool HasEditPermission { get; set; }

        public bool HasDeletePermission { get; set; }
    }
}
