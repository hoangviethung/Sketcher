using System.ComponentModel;

namespace MainProject.Core.Enums
{
    public enum  ActionTypeCollection 
    {
        [Description("Khởi tạo")]
        Temp = 1,

        [Description("Đã tạo mới")]
        Create = 2,

        [Description("Đã chỉnh sửa")]
        Edit = 3,

        [Description("Đã xóa")]
        Delete = 4,

        Approval = 6,

        Reject = 7,

        Answer = 9
    }
}
