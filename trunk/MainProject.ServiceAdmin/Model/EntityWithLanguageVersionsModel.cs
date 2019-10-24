using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ServiceAdmin.Model
{
    public class EntityWithLanguageVersionsModel<T> where T : class
    {
        public T Entity { get; set; }
        public IList<T> LanguageVersions { get; set; }
    }
}
