using MainProject.Core.Commerce;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model.CommerceProperty;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;

namespace MainProject.ServiceAdmin.Helper
{
    public class CommercePropertyHelper
    {
        public static BaseResponseModel CreateOrUpdate(PropertyRepository propertyRepository, PropertyManageModel model,
            LogHistoryService logHistoryService)
        {
            Property propertyEntity = null;
            // Create
            if (model.Id == 0)
            {
                // Bind model to entity
                propertyEntity = new Property
                {
                    Name = model.Name,
                    UnitType = model.UnitTypeSelected,
                    IsPriceEffect = model.IsPriceEffect
                };
                // Save entity to db
                propertyRepository.Insert(propertyEntity);
                // save logHistory
                logHistoryService.Create(new LogHistoryModel() { EntityId = propertyEntity.Id, ActionType = ActionTypeCollection.Create });
            }
            else // Edit
            {
                // Get entity to update
                propertyEntity = propertyRepository.FindUnique(c => c.Id == model.Id);
                // Check entity is exist
                if (propertyEntity == null)
                {
                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.BadRequest,
                        Message = string.Format("<strong style='color:red'>Thuộc tính không tồn tại!</strong>")
                    };
                }
                // Bind model to entity
                propertyEntity.Name = model.Name;
                propertyEntity.UnitType = model.UnitTypeSelected;
                propertyEntity.IsPriceEffect = model.IsPriceEffect;
                // Save entity
                propertyRepository.SaveChanges();
                //save logHistory
                logHistoryService.Create(new LogHistoryModel() { EntityId = propertyEntity.Id, ActionType = ActionTypeCollection.Edit });
            }

            // Save property with property translation
            return new BaseResponseModel() {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Cập nhật thành công!</strong>")
            };

        }
    }
}
