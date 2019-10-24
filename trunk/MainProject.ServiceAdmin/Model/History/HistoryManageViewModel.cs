using MainProject.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ServiceAdmin.Model.History
{
    public class HistoryManageViewModel
    {
        public string ActionBy { get; set; }

        public List<MainProject.Core.LogHistory> ListHistoryContent { get; set; }

        public PagingModel PagingViewModel { get; set; }

        public string Name { get; set; }
    }
}
