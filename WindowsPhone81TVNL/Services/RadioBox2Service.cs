using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;
using WindowsPhoneMultiOSApp.Common.Models;

namespace WindowsPhone81TVNL.Services
{
    public class RadioBox2Service
    {
        private const string BASE_URL = "http://radiobox2.omroep.nl/";

        

        public async static Task<List<AudioFragment>> getAudioFragments(string channelId)
        {
            var client = new HttpClient();
            string message;

            try
            {
                var response = await client.GetStringAsync(BASE_URL + "audiofragment/search.json?q=channel.id:%27" + channelId + "%27%20AND%20available:'1'&order=startdatetime:desc");

                JObject jObject = JObject.Parse(response);
                JArray result = (JArray)jObject["results"];
                List<AudioFragment> fragments = new List<AudioFragment>();

                foreach (JObject item in result)
                {
                    AudioFragment fragment = new AudioFragment();
                    fragment.Title = WebUtility.HtmlDecode(item["description"].ToString());
                    fragment.Url = item["url"].ToString();
                    fragment.Date = DateTime.Parse(item["stopdatetime"].ToString()).ToString("dd-MM-yyyy");
                    fragments.Add(fragment);
                }

                return fragments;
            }
            catch (Exception hrex)
            {
                message = hrex.Message;
            }

            MessageBox.Show(string.Format(ResourceHelper.GetString("WebErrorMessage"), message));
            return new List<AudioFragment>();
        }

        public async static Task<List<Programme>> getNextProgrammes(string channelId, string limit = "10")
        {
            var client = new HttpClient();
            string message;
            
            try
            {
                var response = await client.GetStringAsync(BASE_URL + "broadcast/search.json?q=channel.id:'" + channelId + "'%20AND%20(stopdatetime%3ENOW%20AND%20startdatetime%3CNOW%20OR%20startdatetime%3ENOW)&order=startdatetime:asc&max-results="+limit);

                JObject jObject = JObject.Parse(response);
                JArray result = (JArray)jObject["results"];
                List<Programme> fragments = new List<Programme>();

                foreach (JObject item in result)
                {
                    Programme fragment = new Programme();
                    fragment.Title = item["name"].ToString();
                    if(item["image"] != null) fragment.Image = item["image"]["url"].ToString();
                    string startDateTime = DateTime.Parse(item["startdatetime"].ToString()).ToString("dd-MM, HH:mm");
                    string stopDateTime = DateTime.Parse(item["stopdatetime"].ToString()).ToString("HH:mm");
                    fragment.DateTime = startDateTime + " - " + stopDateTime;
                    fragments.Add(fragment);
                }

                return fragments;
            }
            catch (Exception hrex)
            {
                message = hrex.Message;
            }

            MessageBox.Show(string.Format(ResourceHelper.GetString("WebErrorMessage"), message));
            return new List<Programme>();
        }

        public async static Task<NowOnAir> GetNowOnAir(string channelId)
        {
            var client = new HttpClient();            
            string message;

            try
            {
                NowOnAir OnAir = new NowOnAir();
                var response = await client.GetStringAsync(BASE_URL + "data/radiobox2/nowonair/" + channelId + ".json?t=" + DateTime.Now);

                JObject jObject = JObject.Parse(response);
                JObject result = (JObject)((JArray)jObject["results"])[0];
                OnAir.SongStart = result["startdatetime"].ToString();
                OnAir.SongEnd = result["stopdatetime"].ToString();
                if (result["songfile"] != null)
                {
                    JObject data = (JObject)result["songfile"];
                    OnAir.SongId = data["id"].ToString();
                    OnAir.SongArtist = data["artist"].ToString();
                    OnAir.SongTitle = data["title"].ToString();
                }
                else
                {
                    OnAir.SongArtist = "Onbekend";
                    OnAir.SongTitle = "Onbekend";
                }
                OnAir.SongData = "Nu: " + OnAir.SongTitle + " - " + OnAir.SongArtist;

                return OnAir;
            }
            catch (HttpRequestException hrex)
            {
                message = hrex.Message;
            }

            MessageBox.Show(string.Format(ResourceHelper.GetString("WebErrorMessage"), message));
            return new NowOnAir();
        }

        public async static Task<Broadcast> GetCurrentBroadcast(string channelId)
        {
            var client = new HttpClient();
            string message;

            try
            {
                Broadcast currentBroadcast = new Broadcast();
                var response = await client.GetStringAsync(BASE_URL + "broadcast/search.json?q=channel.id:'" + channelId + "'%20AND%20startdatetime<NOW%20AND%20stopdatetime>NOW&t=" + DateTime.Now);

                try
                {
                    JObject jObject = JObject.Parse(response);
                    JObject result = (JObject)((JArray)jObject["results"])[0];
                    currentBroadcast.Id = (int)result["id"];
                    currentBroadcast.TotalResults = jObject["total-results"].ToString();
                    currentBroadcast.ProgrammeId = result["programme"].ToString();
                    currentBroadcast.ProgrammeName = result["name"].ToString();
                    currentBroadcast.ProgrammeImage = result["image"]["url"].ToString();

                    if (jObject["description"] == null)
                    {
                        var responseProgramme = await client.GetStringAsync(BASE_URL + "programme/rest/" + currentBroadcast.ProgrammeId + ".json");
                        JObject programmeObject = JObject.Parse(responseProgramme);
                        currentBroadcast.ProgrammeDescription = WebUtility.HtmlDecode(programmeObject["description"].ToString());
                    }
                    else
                    {
                        currentBroadcast.ProgrammeDescription = WebUtility.HtmlDecode(result["description"].ToString());
                    }
                    
                    currentBroadcast.ProgrammeDescription = Regex.Replace(currentBroadcast.ProgrammeDescription, @"<(.|\n)*?>", string.Empty);
                    currentBroadcast.BroadcasterName = result["broadcaster"]["name"].ToString();
                    currentBroadcast.BroadcastStart = result["startdatetime"].ToString();
                    currentBroadcast.BroadcastEnd = result["stopdatetime"].ToString();

                    DateTime startDate = DateTime.Parse(currentBroadcast.BroadcastStart);
                    DateTime stopDate = DateTime.Parse(currentBroadcast.BroadcastEnd);
                    string startStopTime = RadioBox2Service.setLength(startDate.Hour.ToString(), 2) + ":" + RadioBox2Service.setLength(startDate.Minute.ToString(), 2) + " - " + RadioBox2Service.setLength(stopDate.Hour.ToString(), 2) + ":" + RadioBox2Service.setLength(stopDate.Minute.ToString(), 2);
                    currentBroadcast.BroadcastTimes = startStopTime;

                    currentBroadcast.Presenters = "";
                    if (result["presenter"] != null)
                    {
                        JArray presenters = (JArray)result["presenter"];

                        for (int i = 0; i < presenters.ToArray().Length; i++)
                        {
                            JObject presenter = (JObject)presenters[i];
                            currentBroadcast.Presenters += presenter["full_name"];
                            if (presenters.ToArray().Length - 2 > i) currentBroadcast.Presenters += ", ";
                            if (presenters.ToArray().Length - 2 == i) currentBroadcast.Presenters += " & ";
                        }
                    }
                
                    if (result["item"] != null)
                    {
                        foreach (JObject item in result["item"])
                        {
                            currentBroadcast.Items.Add(item);
                        }
                    }

                    if (currentBroadcast.ProgrammeDescription.Equals(""))
                    {
                        response = await client.GetStringAsync(BASE_URL + "programme/rest/" + currentBroadcast.ProgrammeId + ".json");
                        jObject = JObject.Parse(response);
                        currentBroadcast.ProgrammeDescription = WebUtility.HtmlDecode(jObject["description"].ToString());
                        currentBroadcast.ProgrammeDescription = Regex.Replace(currentBroadcast.ProgrammeDescription, @"<(.|\n)*?>", string.Empty);
                    }
                }
                catch { }

                return currentBroadcast;
            }
            catch (HttpRequestException hrex)
            {
                message = hrex.Message;
            }

            MessageBox.Show(string.Format(ResourceHelper.GetString("WebErrorMessage"), message));
            return new Broadcast()
            {
                BroadcasterName = "3FM",
                Presenters = "Onbekend",
                ProgrammeName = "Onbekend",
                ProgrammeDescription = "Er is geen informatie beschikbaar."
            };
        }

        public static bool CheckConnection()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                return true;
            }
            return false;
        }

        private static string setLength(string str, int length)
        {
            while (str.Length < length)
            {
                str = "0" + str;
            }
            return str;
        }

    }
}
