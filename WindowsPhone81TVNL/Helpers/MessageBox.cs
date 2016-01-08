using System;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Popups;

namespace WindowsPhone81TVNL.Helpers
{
    public class MessageBox
    {
        public async static Task<bool> Show(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
            return true;
        }

        public async static Task<bool> Show(string title, string message)
        {
            var dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
            return true;
        }
    }
}
