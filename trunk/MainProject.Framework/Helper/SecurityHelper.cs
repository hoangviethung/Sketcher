using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MainProject.Core.Enums;
using System.Web.Mvc;
using MainProject.Framework.ActionFilters;

namespace MainProject.Framework.Helper
{
    public static class SecurityHelper
    {
        public static List<EntityManageTypeObj> ConvertEntityManageTypeToObjList()
        {
            return Enum.GetValues(typeof(EntityManageTypeCollection))
                .Cast<EntityManageTypeCollection>().ToList().Select(x => new EntityManageTypeObj
                {
                    EntityManageType = x,
                    FeatureGroup = x.GetAttribute<EntityManageTypeDefine>().Group,
                    EntityManageTypeName = x.GetAttribute<EntityManageTypeDefine>().Name
                }).ToList();
        }

        public static List<EntityManageTypeObj> GetActiveFeatures(Assembly asm)
        {
            var result = new List<EntityManageTypeObj>();

            var functions = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                .SelectMany(type => type.GetMethods())
                .Where(method =>
                    method.IsPublic
                    && method.IsDefined(typeof(AuthorizeUserAttribute))
                );

            foreach (var item in functions)
            {
                if (item.DeclaringType != null)
                {
                    var dataAttribute = item.GetCustomAttribute<AuthorizeUserAttribute>();
                    var feature = dataAttribute.Feature;
                    var action = dataAttribute.Permission;

                    var featureItem = result.FirstOrDefault(x => x.EntityManageType == feature);
                    if (featureItem == null)
                    {
                        featureItem = new EntityManageTypeObj
                        {
                            EntityManageType = feature,
                            Permissions = new List<PermissionCollection>(),
                            FeatureGroup = feature.GetAttribute<EntityManageTypeDefine>().Group,
                            EntityManageTypeName = feature.GetAttribute<EntityManageTypeDefine>().Name
                        };
                        result.Add(featureItem);
                    }
                    if (featureItem.Permissions.All(x => x != action))
                    {
                        featureItem.Permissions.Add(action);
                    }
                }
            }

            return result;
        }
    }
}
