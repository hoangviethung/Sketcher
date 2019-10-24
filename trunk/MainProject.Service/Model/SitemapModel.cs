using System;
using System.Collections.Generic;
using MainProject.Core;

namespace MainProject.Service.Model
{
    public class SitemapModel
    {
        public string Link { get; set; }
        public DateTime Created { get; set; }
        public string Priority { get; set; }
        public string Title { get; set; }

        public List<Category> listSiteMap { get; set; }
    }
}