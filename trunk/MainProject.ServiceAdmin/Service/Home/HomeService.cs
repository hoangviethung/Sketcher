using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.ServiceAdmin.Model.Home;

namespace MainProject.ServiceAdmin.Service.Home
{
    public class HomeService
    {
        //private readonly ArticleRepository _articleRepository;

        public HomeService(MainDbContext dbContext)
        {
            //_articleRepository = new ArticleRepository(dbContext);
        }

        public HomeViewModel GetIndex()
        {

            return new HomeViewModel()
            {

            };
        }

        
        //public BaseResponseModel BackupDB()
        //{
        //    try
        //    {
        //        var file = string.Empty;
        //        foreach (var item in ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>()
        //                                                                    .Where(v => v.Name != "LocalSqlServer")
        //                                                                    .Select(v => v.Name)
        //                                                                    .ToList())
        //        {
        //            string cs = ConfigurationManager.ConnectionStrings[item].ConnectionString;
        //            // Create folder to save backup
        //            var folder = "/Upload/Backup";
        //            if (!FolderAndFileHelper.CheckFolderExist(folder))
        //            {
        //                FolderAndFileHelper.CreateFolder(folder);
        //            }
        //            // Generate file backup
        //            file = BackupDatabaseHelper.BackupDatabase(cs, folder, item);
        //        }

        //        return new BaseResponseModel
        //        {
        //            Code = HttpStatusCodeCollection.OK,
        //            Message = file,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return new BaseResponseModel
        //    {
        //        Code = HttpStatusCodeCollection.BadRequest,
        //        Message = string.Format("<strong style='color:red'>Xảy ra lỗi trong quá trình backup database. Xin vui lòng thử lại!</strong>")
        //    };
        //}
    }
}
