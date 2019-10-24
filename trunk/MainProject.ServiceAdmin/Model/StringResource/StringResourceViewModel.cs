using MainProject.Core;
using MainProject.Framework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Model.StringResource
{
    public class StringResourceViewModel
    {
        public List<StringResourceValue> ListStringResource { get; set; }

        public int LanguageSelectedValue { get; set; }

        public List<SelectListItem> Languages { get; set; }

        public PagingModel PagingViewModel { get; set; }

        public string Filter { get; set; }

        public StringResourceKey ResourceKey { get; set; }

        public IList<StringResourceValue> ResourceValues { get; set; }
    }
}
