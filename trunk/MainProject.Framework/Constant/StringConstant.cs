using MainProject.Framework.Helper;
namespace MainProject.Framework.Constant
{
    public static class StringUrlConstant
    {
        public static string HomePage
        {
            get { return CultureHelper.GetCurrentLanguage() == "vi" ? "/" : "/"; }
        }
    }

    public static class StringConstant
    {
        public const string DbContextKeyName = "DbContextPerRequest";

        public const string CurrentLanguageKey = "UserCurrentLanguage";

        public const string ViewForRedirectToHomePage = "/Views/Shared/RedirectToHomePage.cshtml";

        public const string ViewForRedirectToAnotherPage = "/Views/Shared/RedirectToAnotherPage.cshtml";

        public const string ViewForErrorPage = "/Views/Shared/Error.cshtml";

        public const string DefaultAdministrator = "Administrator";

        public const string DefaultPassword = "P@ssw0rd";
    }

    public static class RoleName
    {
        public const string Admin = "Admin";

        public const string Mod = "Mod";

        public const string Guest = "Guest";
    }

    public static class NumberConstant
    {
        public const int MinYear = 1789;
    }

    public static class MailConstant
    {
        public const string Server = "SmtpServer";

        public const string Port = "SmtpPort";

        public const string UserName = "UserEmailAccountName";

        public const string Password = "EmailAccountPassword";

        public const string SSL = "EnableSsl";

        public const string EmailCC = "EmailCC";
    }
}
