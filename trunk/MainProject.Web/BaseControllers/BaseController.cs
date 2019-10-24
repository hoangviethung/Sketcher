using System.Web.Mvc;
using MainProject.Data;
using MainProject.Framework.Helper;

namespace MainProject.Web.BaseControllers
{
    public class BaseController : Controller
    {
        private MainDbContext _dbContext;
        private int _currentLanguageId = 0;
        public string cultureKey;

        public MainDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = DalHelper.InvokeDbContext();
                }
                return _dbContext;
            }

        }
        public int CurrentLanguageId
        {
            get
            {
                if (_currentLanguageId == 0)
                {
                    _currentLanguageId = CultureHelper.GetCurrentLanguageId();
                }
                return _currentLanguageId;
            }
        }


        public BaseController()
        {
        }
    }
}
