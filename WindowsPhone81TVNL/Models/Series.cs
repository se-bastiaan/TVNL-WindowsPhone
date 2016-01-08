using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class Series
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("nebo_id")]
        public string NeboId { get; set; }
        //[JsonProperty("mid")]
        //public string Mid { get; set; }
        //[JsonProperty("image")]
        //public string Image { get; set; }
        //[JsonProperty("description")]
        //public string Description { get; set; }
        //[JsonProperty("broadcasters")]
        //public List<string> Broadcasters { get; set; }
        public List<string> Genres { get; set; }
        [JsonProperty("active_episodes_count")]
        public int ActiveEpisodesCount { get; set; }
        [JsonProperty("episodes")]
        public List<Episode> Episodes { get; set; }

        public int Code { get; set; }
    }
}
