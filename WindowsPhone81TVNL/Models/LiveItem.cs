using Newtonsoft.Json;
using QKit.JumpList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WindowsPhone81TVNL.Helpers;

namespace WindowsPhone81TVNL.Models
{
    public class LiveItem
    {
        public LiveItem(string name, string image, string url, bool isRadio)
        {
            Name = name;
            Image = image;
            Url = url;
            IsRadio = isRadio;
        }

        public string Name { get; set; }
        public int Channel { get; set; }
        public string Image { get; set; }
        [JsonProperty("circle_image")]
        public string CircleImage { get; set; }
        public string Url { get; set; }
        [JsonProperty("url_hq")]
        public string HqUrl { get; set; }
        [JsonProperty("url_webcam")]
        public string WebcamUrl { get; set; }
        [JsonProperty("is_radio")]
        public bool IsRadio { get; set; }

        public List<RecentItem> Guide { get; set; }

        public List<RecentItem> TimeGuide
        {
            get
            {
                return (from i in Guide where i.EndTime > DateTime.Now select i).ToList<RecentItem>();
            }
        }

        public IconElement Icon
        {
            get
            {
                if (CircleImage.Contains("play") && !IsRadio)
                {
                    return new SymbolIcon(Symbol.Play);
                }
                else if(IsRadio)
                {
                    return new SymbolIcon(Symbol.MusicInfo);
                }
                else
                {
                    return new SymbolIcon(Symbol.List);
                }
            }
        }

        public List<TimeKeyGroup<RecentItem>> GroupedGuide
        {
            get
            {
                if (Guide == null) return new List<TimeKeyGroup<RecentItem>>();

                List<RecentItem> list = (from i in Guide where i.EndTime > DateTime.Now select i).ToList<RecentItem>();

                return TimeKeyGroup<RecentItem>.CreateGroups(
                    list,
                    (RecentItem i) => { return i.StartTime; },
                    true);
            }
        }
    }
}
