using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Framework.Models
{
    public class IpstackLanguage
    {
        public string code { get; set; }
        public string name { get; set; }
        public string native { get; set; }
    }

    public class IpstackLocation
    {
        public object geoname_id { get; set; }
        public string capital { get; set; }
        public List<IpstackLanguage> languages { get; set; }
        public string country_flag { get; set; }
        public string country_flag_emoji { get; set; }
        public string country_flag_emoji_unicode { get; set; }
        public string calling_code { get; set; }
        public bool is_eu { get; set; }
    }

    public class IpstackLocationModel
    {
        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public object region_code { get; set; }
        public object region_name { get; set; }
        public object city { get; set; }
        public object zip { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public IpstackLocation location { get; set; }
    }
}
