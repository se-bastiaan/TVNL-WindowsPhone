using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;
using Windows.Web.Http;

namespace WindowsPhone81TVNL.Services
{
    public class UitzendingGemistService
    {
        private const string BASE_URL = "http://apps-api.uitzendinggemist.nl/";

        public async static Task<string> GetStringAsync(String url) {
            HttpClient client = CreateClient();
            try {
                string response = await client.GetStringAsync(new Uri(BASE_URL + url));
                client.Dispose();
                return response;
            } catch(Exception e) {
                client.Dispose();
                throw new Exception();
            }
        }

        public async static Task<List<TipItem>> GetTips()
        {
            try
            {
                string response = await GetStringAsync("tips.json");
                List<TipItem> items = JsonConvert.DeserializeObject<List<TipItem>>(response);
                return items;
            }
            catch (Exception e)
            {
                return new List<TipItem>();
            }
        }

        public async static Task<List<RecentItem>> GetRecent()
        {
            try
            {
                string response = await GetStringAsync("broadcasts/recent.json");
                return JsonConvert.DeserializeObject<List<RecentItem>>(response);
            }
            catch (Exception e)
            {
                return new List<RecentItem>();
            }
        }

        // date format: YYYY-MM-DD
        public async static Task<List<RecentItem>> GetRecent(string date)
        {
            try
            {
                string response = await GetStringAsync("broadcasts/" + date + ".json");
                return JsonConvert.DeserializeObject<List<RecentItem>>(response);
            }
            catch (Exception e)
            {
                return new List<RecentItem>();
            }
        }

        public async static Task<Episode> GetEpisode(string whatsonId)
        {
            try
            {
                string response = await GetStringAsync("episodes/" + whatsonId + ".json");
                return JsonConvert.DeserializeObject<Episode>(response);
            }
            catch (Exception e)
            {
                return new Episode();
            }
        }

        public async static Task<Episode> GetSeriesLatestEpisode(string seriesId)
        {
            try
            {
                string response = await GetStringAsync("episodes/series/" + seriesId + "/latest.json");
                return JsonConvert.DeserializeObject<Episode>(response);
            } catch(Exception e) {
                return new Episode();
            }
        }

        public async static Task<Series> GetSeries(string seriesId)
        {
            try
            {
                string response = await GetStringAsync("series/" + seriesId + ".json");
                return JsonConvert.DeserializeObject<Series>(response);
            } catch(Exception e) {
                return new Series();
            }
        }

        public async static Task<List<Episode>> GetPopularEpisodes()
        {
            try
            {
                string response = await GetStringAsync("episodes/popular.json");
                return JsonConvert.DeserializeObject<List<Episode>>(response);
            } catch(Exception e) {
                return new List<Episode>();
            }
        }

        public async static Task<List<Episode>> GetGenreEpisodes(string genre)
        {
            try
            {
                string response = await GetStringAsync("episodes/genre/" + genre + ".json");
                return JsonConvert.DeserializeObject<List<Episode>>(response);
            } catch(Exception e) {
                return new List<Episode>();
            }
        }

        public async static Task<List<Series>> GetPopularSeries()
        {
            try
            {
                string response = await GetStringAsync("series/popular.json");
                return JsonConvert.DeserializeObject<List<Series>>(response);
            } catch(Exception e) {
                return new List<Series>();
            }
        }

        public async static Task<List<Series>> GetSeriesIndex()
        {
            try
            {
                string response = await GetStringAsync("series.json");
                return JsonConvert.DeserializeObject<List<Series>>(response);
            } catch(Exception e) {
                return new List<Series>();
            }
        }

        public async static Task<List<Episode>> SearchEpisodes(string searchString)
        {
            try
            {
                string response = await GetStringAsync("episodes/search/" + Uri.EscapeDataString(searchString) + ".json");
                return JsonConvert.DeserializeObject<List<Episode>>(response);
            } catch(Exception e) {
                return new List<Episode>();
            }
        }

        public async static Task<List<Series>> SuggestSeries(string searchString)
        {
            try
            {
                string response = await GetStringAsync("series/suggest/" + Uri.EscapeDataString(searchString) + ".json");
                return JsonConvert.DeserializeObject<List<Series>>(response);
            }
            catch (Exception e)
            {
                return new List<Series>();
            }
        }

        public async static Task<EncodedStreamData> GetStreamData(string whatsonId)
        {
            try
            {
                string response = await GetStringAsync("odi_plus/android/" + whatsonId + ".json?pub_option=adaptive");
                Debug.WriteLine(response);
                EncodedStreamData data = JsonConvert.DeserializeObject<EncodedStreamData>(response);
                return new EncodedStreamData(data);
            } catch(Exception e) {
                return new EncodedStreamData();
            }
        }

        public async static Task<EncodedStreamData> GetLiveStreamData(string channelId)
        {
            try
            {
                string response = await GetStringAsync("live/android/" + channelId + ".json?extension=Manifest");
                Debug.WriteLine(response);
                EncodedStreamData data = JsonConvert.DeserializeObject<EncodedStreamData>(response);
                return new EncodedStreamData(data);
            } catch(Exception e) {
                return new EncodedStreamData();
            }
        }

        public static string GetVTTUrl(string whatsonId)
        {
            return BASE_URL + "webvtt/" + whatsonId + ".webvtt";
        }

        public async static void PostView(string whatsonId)
        {
            HttpClient client = CreateClient();
            try
            {
                await client.PostAsync(new Uri(BASE_URL + "/episodes/" + whatsonId + "/view"), new HttpStringContent(""));
                client.Dispose();
            }
            catch (Exception e)
            {
                // DO NOTHING
            }
        }

        public static HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(GetUserAgent());
            return client;
        }

        public static string GetUserAgent()
        {
            AssemblyInformation inf = new AssemblyInformation();
            string osVersion = DeviceInformationHelper.GetFirmwareVersion();
            string deviceManufacturer = DeviceInformationHelper.GetDeviceManufacturer();
            string deviceModel = DeviceInformationHelper.GetDeviceModel();
            string userAgent = "NPOxUG/" + inf.Version.ToString() + " WindowsPhone/" + osVersion + " " + deviceManufacturer + "/" + deviceModel;
            return userAgent;
        }
    }
}
