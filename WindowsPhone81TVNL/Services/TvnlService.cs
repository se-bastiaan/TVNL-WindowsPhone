using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;

namespace WindowsPhone81TVNL.Services
{
    public class TvnlService
    {
        private const string BASE_URL = "http://tvnl.se-bastiaan.eu/v2/";

        public async static Task<List<LiveItem>> GetLiveItems()
        {
            HttpClient client = UitzendingGemistService.CreateClient();
            try {
                string response = await client.GetStringAsync(new Uri(BASE_URL + "live_channels.php"));
                client.Dispose();
                return JsonConvert.DeserializeObject<List<LiveItem>>(response);
            } catch(Exception e) {
                client.Dispose();
                return new List<LiveItem>();
            }
        }
    }
}
