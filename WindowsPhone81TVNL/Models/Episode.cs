using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class Episode
    {
        [JsonProperty("whatson_id")]
        public string WhatsonId { get; set; }
        [JsonProperty("mid")]
        public string Mid { get; set; }
        //[JsonProperty("nebo_id")]
        //public string NeboId;
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("broadcasters")]
        public List<string> Broadcasters { get; set; }
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
        [JsonProperty("stills")]
        public List<ImageStill> Stills { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("revoked")]
        public bool Revoked { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("series")]
        public Series Series { get; set; }
        [JsonProperty("video")]
        public List<string> Video { get; set; }
        //[JsonProperty("views")]
        //public int Views { get; set; }
        [JsonProperty("advisories")]
        public List<string> Advisories { get; set; }
        [JsonProperty("broadcasted_at")]
        public int BroadcastedAt { get; set; }
        [JsonProperty("restrictions")]
        public Restrictions Restrictions { get; set; }

        public string Title
        {
            get
            {
                if (Series == null)
                    return Name;
                return Series.Name;
            }
        }

        public string DateTimeLength
        {
            get
            {
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(BroadcastedAt).ToLocalTime();
                int duration = (int)Math.Round((double)Duration / 60);
                return date.ToString("ddd d MMM · HH:mm") + " · " + duration + " min";
            }
        }

        public string ImageStill
        {
            get
            {
                if(Stills != null)
                {
                    return Stills[0].Url;
                } else {
                    return "";
                }
            }
        }
    }
}
