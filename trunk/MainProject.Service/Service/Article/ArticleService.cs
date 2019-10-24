using MainProject.Data;
using MainProject.Data.Repositories;

namespace MainProject.Service.Service.Article
{
    public class ArticleService
    {
        private readonly ArticleRepository _articleRepository;

        public ArticleService(MainDbContext dbContext)
        {
            _articleRepository = new ArticleRepository(dbContext);
        }

        public Core.Article GetDetail(long id)
            => _articleRepository.FindId(id);
    }
}
