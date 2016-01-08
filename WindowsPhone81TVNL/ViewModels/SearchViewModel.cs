using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.Services;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set
            {
                if (value)
                {
                    StatusBarHelper.HideProgressIndicator();
                }
                else
                {
                    StatusBarHelper.ShowProgressIndicator("Zoeken...");
                }
                SetProperty(ref _isDataLoaded, value);
            }
        }

        public SearchViewModel()
        {

        }

        private List<Episode> _episodes;
        public List<Episode> Episodes
        {
            get { return _episodes; }
            set
            {
                SetProperty(ref _episodes, value);
            }
        }

        private List<Series> _series;
        public List<Series> Series
        {
            get { return _series; }
            set
            {
                SetProperty(ref _series, value);
            }
        }

        public bool FirstLoad = true;

        public async void Search(string searchString)
        {
            IsDataLoaded = false;
            Episodes = await UitzendingGemistService.SearchEpisodes(searchString);
            Series = await UitzendingGemistService.SuggestSeries(searchString);
            IsDataLoaded = true;
        }

        public static void NavigateTo()
        {
            ((Frame)Window.Current.Content).Navigate(typeof(SearchPage));
        }
    }
}
