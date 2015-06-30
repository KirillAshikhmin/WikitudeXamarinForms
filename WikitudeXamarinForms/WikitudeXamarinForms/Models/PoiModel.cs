using System;
using Newtonsoft.Json;

namespace Wikitude.Demo.Model
{
    public class PoiModel
    {
        [JsonIgnore]
        public double DistanceMeter;
        [JsonProperty(PropertyName = "id")]
        public string Id;
        [JsonProperty(PropertyName = "name")]
        public string Name;
        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude;
        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude;
        [JsonProperty(PropertyName = "altitude")]
        public double Altitude;
        [JsonProperty(PropertyName = "description")]
        public string Description;
        [JsonProperty(PropertyName = "distance")]
        public string Distance;
        [JsonProperty(PropertyName = "type")]
        public string Type;


        private static readonly Random Rnd = new Random();
        public double NumberDistance { get; set; }

        public double DistanceTo(double latitude, double longitude)
        {
            if (Latitude == null || Longitude == null)
            {
                Longitude = longitude + (-Rnd.NextDouble()/30d + 1d/60d);
                Latitude = latitude + (-Rnd.NextDouble()/30d + 1d/60d);
            }
            const double r = 6371000;
            var a1 = latitude / 180d * Math.PI;
            var a2 = (double)Latitude / 180d * Math.PI;
            var b = ((double)Latitude - latitude) / 180d * Math.PI;
            var c = ((double)Longitude - longitude) / 180d * Math.PI;

            var a = Math.Sin(b / 2d) * Math.Sin(b / 2d) + Math.Cos(a1) * Math.Cos(a2) * Math.Sin(c / 2d) * Math.Sin(c / 2d);
            var n = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = r * n;
            Distance = distance < 1000 ? Math.Truncate(distance) + " m" : distance < 10000 ? Math.Truncate(distance/10)/100 + " km"
                    : distance < 100000 ? Math.Truncate(distance / 100) / 10 + " km" : Math.Truncate(distance / 1000) + " km";
            DistanceMeter = distance;
            return distance;
        }
    }

    public enum PoiTypes
    {
        House,
        Museum
    }
}
