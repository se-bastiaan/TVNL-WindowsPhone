using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class TipItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("published_at")]
        public int PublishedAt { get; set; }
        [JsonProperty("episode")]
        public Episode Episode { get; set; }

        public string DateTimeLength
        {
            get
            {
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Episode.BroadcastedAt).ToLocalTime();
                int duration = (int)Math.Round((double)Episode.Duration / 60);
                return date.ToString("ddd d MMM · HH:mm") + " · " + duration + " min";
            }
        }
    }
}
