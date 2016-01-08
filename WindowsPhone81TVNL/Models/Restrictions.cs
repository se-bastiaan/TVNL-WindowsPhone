using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class Restrictions
    {
        [JsonProperty("age_restriction")]
        public string AgeRestriction { get; set; }
        [JsonProperty("geoIP_restriction")]
        public string GeoRestriction { get; set; }
        class TimeRestriction
        {
            [JsonProperty("online_at")]
            public string OnlineAt { get; set; }
            [JsonProperty("offline_at")]
            public string OfflineAt { get; set; }
        }
    }
}
