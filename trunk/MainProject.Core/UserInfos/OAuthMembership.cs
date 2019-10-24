using System.ComponentModel.DataAnnotations.Schema;

namespace MainProject.Core.UserInfos
{
    [Table("webpages_OAuthMembership")]
    public class OAuthMembership
    {
        public string Provider { get; set; }

        public string ProviderUserId { get; set; }

        public int UserId { get; set; }
    }
}
