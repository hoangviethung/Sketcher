using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Service.LoginHistory
{
    public class LoginHistoryService
    {
        private readonly LogHistoryRepository _logHistoryRepository;
        private static readonly string _folder = "/Upload/LoginHistory";
        private static readonly int PAGE_ITEMS = 20;

        public LoginHistoryService(MainDbContext dbContext)
        {
            _logHistoryRepository = new LogHistoryRepository(dbContext);
        }

        /// <summary>
        /// Get data of login history
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IndexViewModel<Core.LogHistory> GetIndex(string name = null, int page = 1)
        {
            var sql = _logHistoryRepository.Find(x => x.EntityType == EntityTypeCollection.Login);
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql.Where(d => d.ActionBy.Contains(name.Trim()));
            }
            int count = sql.Count();

            return new IndexViewModel<Core.LogHistory>
            {
                ListItems = sql.OrderByDescending(d => d.CreatedDate).Skip((page - 1) * PAGE_ITEMS).Take(PAGE_ITEMS).ToList(),
                PagingViewModel = new PagingModel(count, PAGE_ITEMS, page, "href='/Admin/LoginHistoryAdmin/Index?name=" + name + "&page={0}'"),
                FilterViewModel = new FilterViewModel()
                {
                    BaseUrl = "/Admin/LoginHistory/Index?",
                    FatherSelectModel = new FatherSelectModel()
                    {
                        Fathers = FolderAndFileHelper.GetImages("/Upload/LoginHistory", new[] { _folder })
                                                     .Select(x => new SelectListItem() {
                                                         Text = x
                                                     }).ToList()
                    }
                }
            };
        }
    }
}
