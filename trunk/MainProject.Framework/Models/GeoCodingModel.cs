using Newtonsoft.Json;
using System.Collections.Generic;

namespace MainProject.Framework.Models
{
    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Northeast
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Southwest
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Viewport
    {
        public Northeast Northeast { get; set; }
        public Southwest Southwest { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
        [JsonProperty("location_type")]
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }

    public class Result
    {
        [JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
        public List<string> Types { get; set; }
    }

    public class GeoCodingModel
    {
        public List<Result> Results { get; set; }
        public string Status { get; set; }
    }
}
