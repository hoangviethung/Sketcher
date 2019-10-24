using MainProject.Framework.Models;
using Newtonsoft.Json;
using System;
using System.Net;

namespace MainProject.Framework.Helper
{
    public static class GoogleApiHelper
    {
        // https://cloud.google.com/free/docs/gcp-free-tier
        // https://cloud.google.com/maps-platform/pricing/sheet/
        private static string _key = "AIzaSyCLCUD83RimgdD0qK8MfF5YugpptmqnNxk";

        /// <summary>
        /// Geocoding is the process of converting addresses (like a street address) 
        /// into geographic coordinates (like latitude and longitude),
        /// which you can use to place markers on a map, or position the map.
        /// https://developers.google.com/maps/documentation/geocoding/start
        /// </summary>
        /// <param name="address"></param>
        public static GeoCodingModel GeoCoding(string address)
        {
            try
            {
                string url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, _key);
                WebClient wc = new WebClient();
                wc.Headers["Content-Type"] = "application/json; charset=utf-8";
                var htmlResult = wc.DownloadString(url);

                return JsonConvert.DeserializeObject<GeoCodingModel>(htmlResult);
            }
            catch (Exception ex) {
                return null;
            }
        }

        /// <summary>
        /// ReverseGeocoding is the process of converting geographic coordinates (like latitude and longitude) 
        /// into addresses (like a street address), which you can use to place markers on a map, or position the map.
        /// https://developers.google.com/maps/documentation/geocoding/start
        /// </summary>
        /// <param name="address"></param>
        public static GeoCodingModel ReverseGeoCoding(double lat, double lng)
        {
            try
            {
                string url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={1},{2}&key={0}", _key, lat, lng);
                WebClient wc = new WebClient();
                wc.Headers["Content-Type"] = "application/json; charset=utf-8";
                var htmlResult = wc.DownloadString(url);

                return JsonConvert.DeserializeObject<GeoCodingModel>(htmlResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
