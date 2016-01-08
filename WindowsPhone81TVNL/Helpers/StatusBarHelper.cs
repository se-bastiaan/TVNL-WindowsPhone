using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace WindowsPhone81TVNL.Helpers
{
    public static class StatusBarHelper
    {
        private static StatusBar GetStatusBar()
        {
            return Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        }

        public async static void ShowProgressIndicator(String text)
        {
            StatusBar statusBar = GetStatusBar();
            statusBar.ProgressIndicator.Text = text;
            await statusBar.ProgressIndicator.ShowAsync();
        }

        public async static void HideProgressIndicator()
        {
            await GetStatusBar().ProgressIndicator.HideAsync();
        }
    }
}
