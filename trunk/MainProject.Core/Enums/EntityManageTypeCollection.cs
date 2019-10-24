using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MainProject.Core.Enums
{
    public enum EntityManageTypeCollection
    {
        [EntityManageTypeDefine(FeatureGroup.System, "Quản lý Config")]
        ManageVariables = -1,

        [EntityManageTypeDefine(FeatureGroup.System, "Quản lý Setting")]
        ManageSettings = -2,

        [EntityManageTypeDefine(FeatureGroup.System, "Quản lý Email nhận tin liên hệ")]
        ManageContactEmails = -3,

        [EntityManageTypeDefine(FeatureGroup.System, "Quản lý Tài nguyên")]
        ManageResource = -4,

        [EntityManageTypeDefine(FeatureGroup.Account, "Quản lý Người dùng")]
        ManageUsers = -5,

        [EntityManageTypeDefine(FeatureGroup.Account, "Quản lý Quyền")]
        ManagePermissions = -6,

        [EntityManageTypeDefine(FeatureGroup.Account, "Quản lý Mục lục")]
        ManageMenu = -7,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Danh sách đăng nhập của users")]
        ManageLoginHistory = -8,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Lịch sử hoạt động của users")]
        ManageActivityHistory = -9,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Danh mục")]
        ManageCategories = -10,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Giới thiệu")]
        ManageIntroductions = -11,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Tin tức")]
        ManageNews = -12,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Banner")]
        ManageBanner = -13,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Thư viện")]
        ManageGallery = -14,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Form động")]
        ManageDynamicForm = -15,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Người đăng ký")]
        ManageFormSubscribe = -16,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Liên hệ")]
        ManageContact = -17,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Chi nhánh")]
        ManageBranch = -18,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Đăng ký Email")]
        ManageSubscribe = -19,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Danh mục sản phẩm")]
        ManageCommerceCategory = -20,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Sản phẩm")]
        ManageCommerceProduct = -21,

        [EntityManageTypeDefine(FeatureGroup.Cms, "Quản lý Thuộc tính sản phẩm")]
        ManageCommerceProperty = -22,
    }

    public enum FeatureGroup
    {
        [Description("Tài khoản")]
        Account,

        [Description("Hệ thống")]
        System,

        [Description("CMS")]
        Cms,

        [Description("Giao diện")]
        Layout,

        [Description("Thương mại điện tử")]
        Ecommerce,

        [Description("Thông tin dùng chung")]
        ShareInfo,

        [Description("Dành cho quảng cáo")]
        Advertisement,

        [Description("Tương tác người dùng")]
        CustomerInteraction
    }

    public class EntityManageTypeDefine : Attribute
    {
        public EntityManageTypeDefine() { }

        public EntityManageTypeDefine(FeatureGroup @group, string name)
        {
            Group = @group;
            Name = name;
        }

        public FeatureGroup Group { get; set; }

        public string Name { get; set; }
    }

    public class EntityManageTypeObj
    {
        public EntityManageTypeCollection EntityManageType { get; set; }

        public FeatureGroup FeatureGroup { get; set; }

        public string EntityManageTypeName { get; set; }

        public List<PermissionCollection> Permissions { get; set; }
    }
}
