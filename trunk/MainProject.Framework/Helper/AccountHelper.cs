using MainProject.Framework.Constant;
using System.Web.Security;
using WebMatrix.WebData;

namespace MainProject.Framework.Helper
{
    public static class AccountHelper
    {
        public static string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
            => WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);

        public static bool Login(string userName, string password, bool rememberMe = false)
            =>  WebSecurity.Login(userName, password, persistCookie: rememberMe);

        public static bool CheckUserExist(string userName) => WebSecurity.UserExists(userName);

        public static bool CheckIsAuthenticated() => WebSecurity.IsAuthenticated;

        public static void Logout() => WebSecurity.Logout();

        public static int CurrentUserId() => WebSecurity.CurrentUserId;

        public static string CurrentUserName() => WebSecurity.CurrentUserName;

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
            => WebSecurity.ChangePassword(userName, oldPassword, newPassword);

        public static void Register(string userName, string password, string email, string fullName, string phone)
        {
            // Create Account
            WebSecurity.CreateUserAndAccount(userName, password, new {
                Email = email,
                IsActive = true,
                FullName = fullName,
                Phone = phone
            });
            // Set Role by User name for Account
            AddUserToRole(userName);
            // Login User
            Login(userName, password);
        }

        public static void AddUserToRole(string userName) => Roles.AddUserToRole(userName, RoleName.Guest);

        public static bool IsAdmin() => Roles.IsUserInRole(CurrentUserName(), RoleName.Admin);

        public static void CreateAccount(string userName, string password) => WebSecurity.CreateAccount(userName, password);

        public static string GeneratePassword(string userName)
        {
            var token = WebSecurity.GeneratePasswordResetToken(userName);
            var password = StringHelper.GetString();
            WebSecurity.ResetPassword(token, password);
            return password;
        }
		
        private static SimpleRoleProvider RoleProvider
        {
            get { return (SimpleRoleProvider)Roles.Provider; }
        }

        private static SimpleMembershipProvider MembershipProvider
        {
            get { return (SimpleMembershipProvider)Membership.Provider; }
        }

        /// <summary>
        /// Delete account
        /// </summary>
        /// <param name="userName"></param>
        public static void DeleteAccount(string userName)
            => MembershipProvider.DeleteAccount(userName);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userName"></param>
        public static void DeleteUser(string userName, bool isTrue)
            => MembershipProvider.DeleteUser(userName, isTrue);

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="isTrue"></param>
        public static MembershipUser MemberGetUser(string userName)
            => Membership.GetUser(userName);


        /// <summary>
        /// Get user id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static int GetUserId(string userName)
            => MembershipProvider.GetUserId(userName);

        /// <summary>
        /// Check password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckPassword(string password)
        {
            if (CheckNumberCharacterPassword(password) && CheckSpecialCharacterPassword(password))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check special character password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckSpecialCharacterPassword(string password)
        {
            var specialCharacter = new string[8];
            specialCharacter[0] = "!";
            specialCharacter[1] = "@";
            specialCharacter[2] = "#";
            specialCharacter[3] = "$";
            specialCharacter[4] = "%";
            specialCharacter[5] = "^";
            specialCharacter[6] = "&";
            specialCharacter[7] = "*";
            for (int i = 0; i < specialCharacter.Length; i++)
            {
                if (password.Contains(specialCharacter[i]))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check numberC haracter password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckNumberCharacterPassword(string password)
        {
            var numberCharacter = new string[10];
            numberCharacter[0] = "1";
            numberCharacter[1] = "2";
            numberCharacter[2] = "3";
            numberCharacter[3] = "4";
            numberCharacter[4] = "5";
            numberCharacter[5] = "6";
            numberCharacter[6] = "7";
            numberCharacter[7] = "8";
            numberCharacter[8] = "9";
            numberCharacter[9] = "0";
            for (int i = 0; i < numberCharacter.Length; i++)
            {
                if (password.Contains(numberCharacter[i]))
                    return true;
            }
            return false;
        }
    }
}
