using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using WindowsPhone81TVNL.Models;

namespace WindowsPhone81TVNL.Services
{
    public class OdiService
    {
        public async static Task<UriShieldData> getUriShieldData(string id, bool live)
        {
            EncodedStreamData data;
            UriShieldData shieldData;
            if (live)
            {
                data = await UitzendingGemistService.GetLiveStreamData(id);
                shieldData = new UriShieldData();
                shieldData.Subtitles = false;
                shieldData.Url = await GetVideoStream(data.PlainText);
                return shieldData;
            }
            else
            {
                data = await UitzendingGemistService.GetStreamData(id);
                HttpClient client = UitzendingGemistService.CreateClient();
                try
                {
                    Debug.WriteLine(data.PlainText);
                    string urishield_json = await client.GetStringAsync(new Uri(data.PlainText + "?extension=Manifest"));
                    Debug.WriteLine(urishield_json);
                    shieldData = JsonConvert.DeserializeObject<UriShieldData>(urishield_json);
                    shieldData.Subtitles = data.Subtitles;
                    client.Dispose();
                    Debug.WriteLineIf(shieldData != null, shieldData.Url);
                    return shieldData;
                } catch(Exception e) {
                    client.Dispose();
                    return new UriShieldData();
                }
            }
        }

        public async static Task<string> GetVideoStream(string url)
        {
            HttpClient client = UitzendingGemistService.CreateClient();
            try
            {
                if (!url.Contains("?"))
                {
                    url += "?type=json&protection=url";
                }
                else
                {
                    url += "&type=json&protection=url";
                }
                string response = await client.GetStringAsync(new Uri(url));
                Debug.WriteLine(response);
                client.Dispose();
                return JsonConvert.DeserializeObject<string>(response);
            }
            catch (Exception e)
            {
                client.Dispose();
                return "";
            }
        }
    }
}
