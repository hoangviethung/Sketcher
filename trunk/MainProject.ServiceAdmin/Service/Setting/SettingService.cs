using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Setting;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Collections.Generic;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Setting
{
    public class SettingService
    {
        private readonly SettingRepository _settingRepository;
        private readonly LogHistoryService _logHistoryService;

        public SettingService(MainDbContext dbContext, string currenUser)
        {
            _settingRepository = new SettingRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, Core.Enums.EntityTypeCollection.Setting);
        }

        /// <summary>
        /// Get index setting
        /// </summary>
        /// <returns></returns>
        public List<Core.Setting> GetIndex()
        {
            return _settingRepository.FindAll().ToList();
        }

        /// <summary>
        /// Get setting for setting edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<SettingManageViewModel> Edit(long id)
        {
            var entity = _settingRepository.FindUnique(x => x.Id == id);
            if (entity == null)
            {
                return new BaseResponseModel<SettingManageViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("Không tìm thấy đối tượng cần sửa")
                };
            }

            return new BaseResponseModel<SettingManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = new SettingManageViewModel(entity)
            };
        }

        /// <summary>
        /// Update data setting into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel Update(SettingManageViewModel model)
        {
            if (_settingRepository.CheckDuplicateKey(model.Key, model.Id))
            {
                // Get entity to update
                var entity = _settingRepository.FindUnique(d => d.Id == model.Id);
                if (entity != null)
                {
                    // Bind data from model to entity
                    SettingManageViewModel.ToEntity(ref entity, model);
                    // Save log history
                    _logHistoryService.Create(new LogHistoryModel { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

                    return new BaseResponseModel()
                    {
                        Code = HttpStatusCodeCollection.OK,
                        Message = string.Format("<strong style='color:green'>Cập nhật thành công!</strong>")
                    };
                }

                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Đối tượng không tồn tại!</strong>")
                };
            }

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = string.Format("<strong style='color:red'>Key đã tồn tại!</strong>")
            };
        }

        /// <summary>
        /// Delete object setting from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public string Delete(int id)
        //{
        //    var setting = _settingRepository.FindUnique(x => x.Id == id);
        //    if (setting == null)
        //        return string.Format("Có lỗi xảy ra không thể xóa được!");
        //    _settingRepository.Delete(setting);
        //    _logHistoryService.Insert(new Model.LoHistory.LogHistoryModel() { EntityId = setting.Id, ActionType = ActionTypeCollection.Delete });
        //    return string.Format("<strong style='color:green'>Xóa thành công!</strong>");
        //}

    }
}
