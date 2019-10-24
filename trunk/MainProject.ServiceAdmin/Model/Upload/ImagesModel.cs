using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.ServiceAdmin.Model.Upload
{
    public class ImagesModel
    {
        public ImagesModel() { Images = new List<ImageInfo>(); }

        public List<ImageInfo> Images { get; set; }

        public string ImageFolder { get; set; }
    }

    public class ImageInfo
    {
        public string Name { get; set; }

        public string Link { get; set; }
    }
}
