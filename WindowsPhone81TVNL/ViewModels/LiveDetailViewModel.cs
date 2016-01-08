using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.ViewModels
{
    public class LiveDetailViewModel : BaseViewModel
    {
        private LiveItem _item;
        public LiveItem Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public void LoadData(LiveItem item)
        {
            IsDataLoaded = false;
            Item = item;
            IsDataLoaded = true;
        }

        public static void NavigateTo(LiveItem item)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(LiveDetailPage), item);
        }
    }
}
