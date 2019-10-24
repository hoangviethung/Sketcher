using MainProject.Core;
using MainProject.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.Menu
{
   public class MenuItemManageModel
    {
        public long Id { get; set; }

        public long LogHistoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề!")]
        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string Title { get; set; }

        public int Order { get; set; }

        [StringLength(500, ErrorMessage = "Không được vượt quá 500 ký tự!")]
        public string Link { get; set; }

        public LinkTargetCollection LinkTargetSelected { get; set; }

        public List<SelectListItem> LinkTargetSelectListItems { get; set; }

        public long ParentSelectedId { get; set; }

        public List<SelectListItem> Parents { get; set; }

        public int LanguageSelectedValue { get; set; }

        public List<SelectListItem> LanguageSelectListItems { get; set; }

        public long MenuSelectedValue { get; set; }

        public List<SelectListItem> MenuSelectListItems { get; set; }

        public bool IsEdit { get; set; }

        [StringLength(200, ErrorMessage = "Không được vượt quá 200 ký tự!")]
        public string ImageUrl { get; set; }

        public MenuItemManageModel() { }

        public MenuItemManageModel(MenuItem entity)
        {
            Id = entity.Id;
            LanguageSelectedValue = entity.Language.Id;
            Title = entity.Title;
            Link = entity.Link;
            LinkTargetSelected = entity.LinkTarget;
            MenuSelectedValue = entity.Menu.Id;
            Order = entity.Order;
            ParentSelectedId = entity.Parent != null ? entity.Parent.Id : 0;
            ImageUrl = entity.ImageUrl;
        }

        public static void ToEntity(MenuItemManageModel model, ref MenuItem entity)
        {
            entity.Title = model.Title;
            entity.Link = model.Link;
            entity.Order = model.Order;
            entity.ImageUrl = model.ImageUrl;
            entity.LinkTarget = model.LinkTargetSelected;
        }
    }
}
