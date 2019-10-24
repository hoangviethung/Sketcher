using System;
using System.Linq;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Data.Repositories;
using MainProject.Framework.ActionResults;
using MainProject.Framework.Helper;

namespace MainProject.Web.BaseControllers.Ultities
{
    public class ServiceController : BaseController
    {
        private readonly CategoryRepository _categoryRepository;

        public ServiceController()
        {
            _categoryRepository = new CategoryRepository(DbContext);
        }

        public string ReGenerateDb()
        {
            //generate db
            var message = Framework.DatabaseFramework.ReGenerateDb.Init();
            
            if (!string.IsNullOrEmpty(message))
                return message;
            return "Re generate database successfully!";
        }
    }
}
