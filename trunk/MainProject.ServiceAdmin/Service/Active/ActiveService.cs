using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Active
{
    public class ActiveService
    {
        private readonly LogHistoryRepository _logHistoryRepository;
        private readonly int PAGE_ITEMS = 20;

        public ActiveService(MainDbContext dbContext)
        {
            _logHistoryRepository = new LogHistoryRepository(dbContext);
        }
       
        public IndexViewModel<Core.LogHistory> GetIndex(string name = null, int page = 1)
        {
            var sql = _logHistoryRepository.Find(x => x.EntityType != EntityTypeCollection.Login);

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = sql.Where(d => d.ActionBy.Contains(name.Trim()));
            }
            int count = sql.Count();

            return new IndexViewModel<Core.LogHistory>
            {
                ListItems = sql.OrderByDescending(d => d.CreatedDate).Skip((page - 1) * PAGE_ITEMS).Take(PAGE_ITEMS).ToList(),
                PagingViewModel = new PagingModel(count, PAGE_ITEMS, page, "href='/Admin/ActiveHistoryAdmin/Index?name=" + name + "&page={0}'"),
                FilterViewModel = new FilterViewModel()
                {
                    BaseUrl = "/Admin/ActiveHistoryAdmin/Index?",
                }
            };
        }
    }
}
