using MainProject.Core.Enums;
using MainProject.Core.UserInfos;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Role;
using MainProject.ServiceAdmin.Service.LogHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ServiceAdmin.Service.Role
{
   public class RoleService
    {
        private readonly RoleRepository _roleRepository;
        private readonly LogHistoryService _logHistoryService;

        public RoleService(MainDbContext dbContext, string currentUser)
        {
            _roleRepository = new RoleRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext,EntityTypeCollection.Role);
        }

        public IndexViewModel<Core.UserInfos.Role> GetIndex()
        {
            var roles = _roleRepository.FindAll().ToList();
            var model = new IndexViewModel<Core.UserInfos.Role>()
            {
                ListItems = roles,
                PagingViewModel = new PagingModel(roles.Count, roles.Count, 1, "href='#'")

            };

            return model;
        }

        public RoleManageModel Manage(int? id)
        {
            Core.UserInfos.Role role = null;
            if (id != null && id != 0)
            {
                role = _roleRepository.FindUnique(x => x.RoleId == id);
            }
            var model = role != null ? new RoleManageModel(role) : new RoleManageModel();

            model.BuildActiveFeatures(Assembly.GetCallingAssembly());

            return model;
        }

        public void Manage(RoleManageModel model)
        {
            var role = _roleRepository.FindUnique(x => x.RoleId == model.RoleId);
            if (role == null)
            {
                role = _roleRepository.CreateRole(model.RoleName, model.RoleDescription);     
                _logHistoryService.Insert(new LogHistoryModel { ActionType = ActionTypeCollection.Create, EntityId = role.RoleId });
            }
            else
            {
                role.RoleName = model.RoleName;
                role.RoleDescription = model.RoleDescription;
                _logHistoryService.Insert(new LogHistoryModel { ActionType = ActionTypeCollection.Edit, EntityId = role.RoleId });
            }

            UpdateFeaturePermission(role.RolePrivilleges, model.ViewContentIds, PermissionCollection.View);
            UpdateFeaturePermission(role.RolePrivilleges, model.CreateContentIds, PermissionCollection.Create);
            UpdateFeaturePermission(role.RolePrivilleges, model.EditContentIds, PermissionCollection.Edit);
            UpdateFeaturePermission(role.RolePrivilleges, model.DeleteContentIds, PermissionCollection.Delete);
            _roleRepository.SaveChanges();

        }

        public string Delete(int id)
        {
            _logHistoryService.Insert(new LogHistoryModel { ActionType = ActionTypeCollection.Delete, EntityId = id });
            return string.Format(_roleRepository.RemoveRole(id));
        }
        private void UpdateFeaturePermission(List<RolePrivillege> rolePrivilleges, List<int> features,
          PermissionCollection permission)
        {
            var exitsFeatures = rolePrivilleges.Where(c => c.PermissionType == permission).ToList();
            foreach (var featureValue in features)
            {
                EntityManageTypeCollection feature = (EntityManageTypeCollection)featureValue;
                if (exitsFeatures.All(x => x.FeatureType != feature))
                {
                    rolePrivilleges.Add(new RolePrivillege
                    {
                        FeatureType = feature,
                        PermissionType = permission
                    });
                }
            }
            foreach (var exitsFeature in exitsFeatures)
            {
                if (features.All(x => x != (int)exitsFeature.FeatureType))
                {
                    _roleRepository.RemoveRolePrivillege(exitsFeature.Id);
                    rolePrivilleges.Remove(exitsFeature);
                }
            }
        }
    }
}
