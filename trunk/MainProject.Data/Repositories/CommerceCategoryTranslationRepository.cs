using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainProject.Core.Commerce;

namespace MainProject.Data.Repositories
{
    public class CommerceCategoryTranslationRepository : AbstractMainProjectRepository<CommerceCategoryTranslation>
    {
        public CommerceCategoryTranslationRepository(MainDbContext db) : base(db)
        {
        }
    }
}
