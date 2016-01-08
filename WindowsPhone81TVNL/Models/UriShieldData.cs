using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class UriShieldData
    {
        [JsonProperty("errorcode")]
        public string ErrorCode;
        [JsonProperty("errorstring")]
        public string ErrorString;
        [JsonProperty("family")]
        public string Family;
        [JsonProperty("path")]
        public string Path;
        [JsonProperty("protocol")]
        public string Protocol;
        [JsonProperty("server")]
        public string Server;
        [JsonProperty("wait")]
        public int Wait;
        [JsonProperty("url")]
        public string Url;
        public bool Subtitles;
    }
}
