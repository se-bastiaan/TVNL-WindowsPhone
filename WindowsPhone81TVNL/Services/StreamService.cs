using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;

namespace WindowsPhone81TVNL.Services
{
    public class StreamService
    {
        public async static Task<UriShieldData> GetVideoStream(string url)
        {
            var client = new HttpClient();
            string message;

            try
            {
                if (url.EndsWith("Manifest"))
                {
                    url += "?type=json&protection=url";
                }
                else
                {
                    url += "&type=json&protection=url";
                }
                var response = await client.GetStringAsync(new Uri(url));
                UriShieldData data = new UriShieldData();
                data.Subtitles = false;
                data.Url = JsonConvert.DeserializeObject<string>(response);

                client.Dispose();

                return data;
            }
            catch (Exception hrex)
            {
                message = hrex.Message;
                client.Dispose();
            }

            MessageBox.Show(string.Format(ResourceHelper.GetString("WebErrorMessage"), message));
            return new UriShieldData();
        }
    }
}
