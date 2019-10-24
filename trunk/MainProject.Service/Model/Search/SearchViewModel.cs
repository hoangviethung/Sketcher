using System;
using System.Collections.Generic;
using MainProject.Framework.Models;

namespace MainProject.Service.Model
{
    /// <summary>
    /// Search page model 
    /// </summary>
    public class SearchViewModel
    {
        // Home page slides
        public List<SearchObject> SearchObjects { get; set; }

        // Paging model
        public PagingModel PagingModel { get; set; }

        // Search text
        public string Text { get; set; }

        // Total results
        public int TotalResults { get; set; }
    }

    public class SearchObject
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public int ViewCount { get; set; }

        public DateTime CreadtedDate { get; set; }
    }
}