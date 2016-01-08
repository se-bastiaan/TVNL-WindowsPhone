using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class ImageStill
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}
