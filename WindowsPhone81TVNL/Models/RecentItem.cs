using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhone81TVNL.Models
{
    public class RecentItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("rerun")]
        public bool Rerun { get; set; }
        [JsonProperty("starts_at")]
        public int StartAt { get; set; }
        [JsonProperty("ends_at")]
        public int EndsAt { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("episode")]
        public Episode Episode { get; set; }

        public DateTime EndTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(EndsAt).ToLocalTime();
            }
        }

        public DateTime StartTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(StartAt).ToLocalTime();
            }
        }

        public string TimeText
        {
            get
            {
                return StartTime.ToString("HH:mm");
            }
        }

        public string DurationText
        {
            get
            {
                if (Duration <= 3600)
                {
                    int duration = (int)Math.Round((double)Duration / 60);
                    return duration + " min";
                }
                else
                {
                    int duration = (int)Math.Round((double)Duration / 60);
                    int hours = (int)Math.Floor((double) duration / 60);
                    int minutes_left = duration - (hours * 60);

                    return hours + " uur en " + minutes_left + " min";
                }
            }
        }

        public string DateTimeLength
        {
            get
            {
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(StartAt).ToLocalTime();
                int duration = (int)Math.Round((double)Duration / 60);
                return date.ToString("ddd d MMM · HH:mm") + " · " + duration + " min";
            }
        }
    }
}
