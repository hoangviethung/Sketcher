using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MainProject.Core.Enums;
using MainProject.Framework.Helper;

namespace MainProject.ServiceAdmin.Model.Role
{
    public class RoleManageModel
    {
        public RoleManageModel()
        {
            InitDefaultList();
        }

        public RoleManageModel(Core.UserInfos.Role role)
        {
            RoleId = role.RoleId;
            RoleName = role.RoleName;
            RoleDescription = role.RoleDescription;
            InitDefaultList();

            ViewContentIds = role.RolePrivilleges.Where(x => x.PermissionType == PermissionCollection.View)
                .Select(x => (int)x.FeatureType).ToList();
            CreateContentIds = role.RolePrivilleges.Where(x => x.PermissionType == PermissionCollection.Create)
                .Select(x => (int)x.FeatureType).ToList();
            EditContentIds = role.RolePrivilleges.Where(x => x.PermissionType == PermissionCollection.Edit)
                .Select(x => (int)x.FeatureType).ToList();
            DeleteContentIds = role.RolePrivilleges.Where(x => x.PermissionType == PermissionCollection.Delete)
                .Select(x => (int)x.FeatureType).ToList();
        }

        private void InitDefaultList()
        {
            ViewContentIds = new List<int>();
            CreateContentIds = new List<int>();
            EditContentIds = new List<int>();
            DeleteContentIds = new List<int>();
            ContentIds = new List<int>();
            FeatureGroups = new List<FeatureGroupViewModel>();
        }

        public int RoleId { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Độ dài tài khoản không quá 50 ký tự!")]
        [Required(ErrorMessage = "Vui lòng nhập tên nhóm tài khoản!")]
        [RegularExpression("([0-9a-zA-Z]+)", ErrorMessage = "Tên nhóm tài khoản không được có ký tự đặc biệt!")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả cho nhóm tài khoản!")]
        public string RoleDescription { get; set; }

        public List<int> ViewContentIds { get; set; }

        public List<int> CreateContentIds { get; set; }

        public List<int> EditContentIds { get; set; }

        public List<int> DeleteContentIds { get; set; }

        public List<int> ContentIds { get; set; }

        public List<FeatureGroupViewModel> FeatureGroups { get; set; }

        public void BuildActiveFeatures(Assembly asm)
        {
            FeatureGroups = new List<FeatureGroupViewModel>();

            var features = SecurityHelper.GetActiveFeatures(asm);
            foreach (var feature in features)
            {
                var featureGroup = FeatureGroups.FirstOrDefault(x => x.FeatureGroup == feature.FeatureGroup);
                if (featureGroup == null)
                {
                    featureGroup = new FeatureGroupViewModel
                    {
                        FeatureGroup = feature.FeatureGroup,
                        FeatureGroupName = feature.FeatureGroup.GetDescription(),
                        Features = new List<FeatureViewModel>()
                    };
                    FeatureGroups.Add(featureGroup);
                }

                var featureVm = new FeatureViewModel
                {
                    FeatureName = feature.EntityManageTypeName,
                    FeatureValue = (int)feature.EntityManageType,
                    HasViewPermission = feature.Permissions.Contains(PermissionCollection.View),
                    HasCreatePermission = feature.Permissions.Contains(PermissionCollection.Create),
                    HasEditPermission = feature.Permissions.Contains(PermissionCollection.Edit),
                    HasDeletePermission = feature.Permissions.Contains(PermissionCollection.Delete)
                };

                featureGroup.Features.Add(featureVm);

                var allPermissionExist = true;
                if (featureVm.HasViewPermission)
                {
                    allPermissionExist = ViewContentIds.Any(x => x == (int)feature.EntityManageType);
                }
                if (allPermissionExist && featureVm.HasCreatePermission)
                {
                    allPermissionExist = CreateContentIds.Any(x => x == (int)feature.EntityManageType);
                }
                if (allPermissionExist && featureVm.HasEditPermission)
                {
                    allPermissionExist = EditContentIds.Any(x => x == (int)feature.EntityManageType);
                }
                if (allPermissionExist && featureVm.HasDeletePermission)
                {
                    allPermissionExist = DeleteContentIds.Any(x => x == (int)feature.EntityManageType);
                }
                if (allPermissionExist)
                {
                    ContentIds.Add((int)feature.EntityManageType);
                }
            }
        }
    }
}
