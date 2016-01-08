using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Web.Http;

namespace WindowsPhone81TVNL.Helpers
{
    public static class UpdateHelper
    {
        public static async Task CheckUpdate()
        {
            const string storeAppDetailsUri = "http://marketplaceedgeservice.windowsphone.com/v8/catalog/apps/b658425e-ba4c-4478-9af3-791fd0f1abfe?os={0}&cc={1}&lang={2}";

            var updatedAvailable = false;

            try
            {
                var osVersion = "8.1";
                var lang = CultureInfo.CurrentCulture.Name;
                var countryCode = lang.Length == 5 ? lang.Substring(3) : "US";

                using (var message = new HttpRequestMessage(HttpMethod.Get, new Uri(string.Format(storeAppDetailsUri, osVersion, countryCode, lang))))
                {
                    message.Headers.UserAgent.ParseAdd("Windows Mobile 8.1");

                    using (var client = new HttpClient())
                    {
                        var response = await client.SendRequestAsync(message);
                        if (response.StatusCode != HttpStatusCode.Ok) return;

                        var stream = await response.Content.ReadAsStringAsync();
                        
                            XNamespace atom = "http://www.w3.org/2005/Atom";
                            XNamespace apps = "http://schemas.zune.net/catalog/apps/2008/02";

                            var doc = XDocument.Parse(stream);
                            if (doc.Document == null) return;

                            var entry = doc.Document.Descendants(atom + "feed")
                                .Descendants(atom + "entry")
                                .FirstOrDefault();

                            if (entry == null) return;

                            var versionElement = entry.Elements(apps + "version").FirstOrDefault();
                            if (versionElement == null) return;

                            Version storeVersion;

                            if (Version.TryParse(versionElement.Value, out storeVersion))
                            {
                                AssemblyInformation assembly = new AssemblyInformation();
                                var currentVersion = assembly.Version;

                                updatedAvailable = storeVersion > currentVersion;
                            }
                        }
                    }
                
            }
            catch (Exception ex)
            {
                // HANDLE ERROR HERE. THERE IS NO POINT IN SHOWING USER A MESSAGE. GOOD PLACE TO SEND SILENT ERROR REPORT OR JUST SWALLOW THE EXCEPTION.
                Debug.WriteLine(ex);
            }

            if (updatedAvailable)
            {
                await MessageBox.Show("Update beschikbaar", "Er is een nieuwe update beschikbaar in de Windows Phone Store.");
            }
        }
    }
}
