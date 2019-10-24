using System.Collections.Generic;
using MainProject.Framework.Models;

namespace MainProject.ServiceAdmin.Model
{
    public class IndexViewModel<T> where T : class
    {
        public IList<T> ListItems { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public PagingModel PagingViewModel { get; set; }
        
    }
}