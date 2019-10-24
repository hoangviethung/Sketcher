using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MainProject.ServiceAdmin.Model.Upload
{
    public class UploadImageManageViewModel
    {
        [Display(Name = "Internet URL")]
        public string Url { get; set; }

        public bool IsUrl { get; set; }

        [Display(Name = "Flickr image")]
        public string Flickr { get; set; }

        public bool IsFlickr { get; set; }

        [Display(Name = "Local file")]
        public HttpPostedFileBase File { get; set; }

        public bool IsFile { get; set; }

        [Range(0, int.MaxValue)]
        public int X { get; set; }

        [Range(0, int.MaxValue)]
        public int Y { get; set; }

        [Range(1, int.MaxValue)]
        public int Width { get; set; }

        [Range(1, int.MaxValue)]
        public int Height { get; set; }

        public string FolderName { get; set; }

        [Display(Name = "Thêm watermark?")]
        public bool IsWaterMark { get; set; }

        public ImagesModel Images { get; set; }
    }
}
