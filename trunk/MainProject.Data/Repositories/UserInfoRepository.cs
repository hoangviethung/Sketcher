using MainProject.Core.UserInfos;
using System.Collections.Generic;
using System.Linq;

namespace MainProject.Data.Repositories
{
    public class UserInfoRepository : AbstractMainProjectRepository<UserProfile>
    {
        private MainDbContext _dbContext;

        public UserInfoRepository(MainDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public UserProfile FindId(int id)
        {
            return FindUnique(c => c.UserId == id);
        }

        public string DeleteUserProfile(UserProfile userProfile)
        {
            _dbContext.UserProfiles.Remove(userProfile);
            if (_dbContext.SaveChanges() > 0)
            {
                return "Xóa thành công";
            }
            else
            {
                return "Xảy ra sự cố trong quá trình xóa dữ liệu, xin vui lòng thử lại";
            }
        }

        public IQueryable<UserProfile> GetUserProFiles(string userName, int subSiteId)
        {
            IQueryable<UserProfile> userProfiles = null;
            userProfiles = (from user in _dbContext.UserProfiles
                                         where (string.IsNullOrEmpty(userName) || user.UserName.Contains(userName))
                            select user);

            return userProfiles;
        }

        public bool CheckEmailExist(string email)
            => _dbContext.UserProfiles.Any(x => x.Email.Equals(email));

        public void UpdatePassword(int userId, string password)
        {
            var user = FindId(userId);
            user.GeneratePassword = password;
            SaveChanges();
        }
    }
}
