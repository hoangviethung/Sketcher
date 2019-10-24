using MainProject.Core;
using MainProject.Framework.Models;
using System;
using System.Collections.Generic;

namespace MainProject.Service.Model.Gallery
{
    public class CategoryViewModel
    {
        public class GalleryViewModel
        {
            public Category Category { get; set; }

            public List<GalleryItemViewModel> Albums { get; set; }

            public int Total { get; set; }

            public int ImageTotal { get; set; }

            public int VideoTotal { get; set; }

            public PagingModel PagingModel { get; set; }
        }

        public class GalleryItemViewModel
        {
            public string Name { get; set; }

            public string ImageDefault { get; set; }

            public DateTime Order { get; set; }

            public List<Media> Medias { get; set; }
        }
    }
}