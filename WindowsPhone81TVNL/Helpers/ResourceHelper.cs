using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhone81TVNL.Helpers
{
    public class ResourceHelper
    {
        public static string GetString(string key) {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            return loader.GetString(key);
        }
    }
}
