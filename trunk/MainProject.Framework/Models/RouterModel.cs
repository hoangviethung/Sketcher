using MainProject.Core;

namespace MainProject.Framework.Models
{
    public class RouterModel
    {
        public RouterModel()
        {
            HasResult = false;
            UrlRecord = null;
            PageIndex = 0;
            UrlRequest = string.Empty;
        }

        public bool HasResult { get; set; }

        public UrlRecord UrlRecord { get; set; }

        public int PageIndex { get; set; }

        public string UrlRequest { get; set; }

        public string OtherParams { get; set; }
    }
}
