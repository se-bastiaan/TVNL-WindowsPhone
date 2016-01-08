using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class Broadcast
    {
        public int Id { get; set; }
        public string TotalResults { get; set; }
        public string ProgrammeId { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeWebsite { get; set; }
        public string ProgrammeEmail { get; set; }
        public string ProgrammePhone { get; set; }
        public string ProgrammeImage { get; set; }
        public string ProgrammeDescription { get; set; }
        public string BroadcasterName { get; set; }
        public string BroadcastStart { get; set; }
        public string BroadcastEnd { get; set; }
        public string BroadcastTimes { get; set; }
        public List<JObject> Items { get; set; }
        public string Presenters { get; set; }
    }
}
