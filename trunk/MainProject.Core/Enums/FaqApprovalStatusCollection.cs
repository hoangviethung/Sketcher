using System.ComponentModel;

namespace MainProject.Core.Enums
{
    public enum FaqApprovalStatusCollection
    {
        [Description("Tất cả")]
        NotPending = 1,

        [Description("Vừa gửi tới")]
        JustSent = 2,

        [Description("Chờ chấp nhận")]
        Pending = 3,

        [Description("Chấp nhận đăng")]
        Accept = 4,

        [Description("Từ chối đăng")]
        Reject = 5
    }
}
