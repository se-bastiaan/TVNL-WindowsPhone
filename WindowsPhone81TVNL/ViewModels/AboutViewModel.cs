using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPhone81TVNL.Commands;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ReviewCommand Review { get; set; }
        public ContactCommand Contact { get; set; }

        public AboutViewModel()
        {
            Review = new ReviewCommand();
            Contact = new ContactCommand();
        }

        public static void NavigateTo()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(AboutPage));
        }
    }
}
