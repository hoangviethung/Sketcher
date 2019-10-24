using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Setting
{
    public class SettingManageViewModel
    {
        public SettingManageViewModel() { }

        public SettingManageViewModel(Core.Setting setting)
        {
            Id = setting.Id;
            Key = setting.Key;
            Value = setting.Value;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập key")]
        [RegularExpression("([a-zA-Z._-]+)", ErrorMessage = "Vui lòng không nhập ký tự đặc biệt!")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Nhập value")]
        [AllowHtml]
        public string Value { get; set; }

        public static void ToEntity(ref Core.Setting entity, SettingManageViewModel model)
        {
            entity.Key = model.Key;
            entity.Value = model.Value;
        }
    }
}
