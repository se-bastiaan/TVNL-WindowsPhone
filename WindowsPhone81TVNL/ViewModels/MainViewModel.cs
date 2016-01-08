using QKit.JumpList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPhone81TVNL.Commands;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.Services;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private List<TipItem> _tips;
        public List<TipItem> Tips
        {
            get { return _tips; }
            set { SetProperty(ref _tips, value); }
        }

        private List<RecentItem> _new;
        public List<RecentItem> New
        {
            get { return _new; }
            set { SetProperty(ref _new, value); }
        }

        private List<Episode> _popular;
        public List<Episode> Popular
        {
            get { return _popular; }
            set { SetProperty(ref _popular, value); }
        }

        private List<JumpListGroup<Series>> _index;
        public List<JumpListGroup<Series>> Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        private List<LiveItem> _live;
        public List<LiveItem> Live
        {
            get { return _live; }
            set { SetProperty(ref _live, value); }
        }

        public AboutCommand AboutCommand { get; set; }
        public AudioPlayPauseCommand AudioPlayPauseCommand { get; set; }
        public AudioStopCommand AudioStopCommand { get; set; }
        public SearchCommand SearchCommand { get; set; }
        public SupportCommand SupportCommand { get; set; }

        private double _tileSize;
        public double TileSize
        {
            get { return _tileSize; }
            set { SetProperty(ref _tileSize, value); }
        }

        private int _hasAudio;
        public int HasAudio
        {
            get { return _hasAudio; }
            set { SetProperty(ref _hasAudio, value); }
        }

        private Uri _appBarplayButtonIcon;
        public Uri AppBarPlayButtonIcon
        {
            get { return _appBarplayButtonIcon; }
            set { SetProperty(ref _appBarplayButtonIcon, value); }
        }

        private string _appBarplayButtonText;
        public string AppBarPlayButtonText
        {
            get { return _appBarplayButtonText; }
            set { SetProperty(ref _appBarplayButtonText, value); }
        }

        public bool IsTipsLoading = false;
        public bool IsNieuwLoading = false;
        public bool IsPopulairLoading = false;
        public bool IsIndexLoading = false;
        public bool IsLiveLoading = false;

        public MainViewModel()
        {
            AboutCommand = new AboutCommand();
            AudioPlayPauseCommand = new AudioPlayPauseCommand();
            AudioStopCommand = new AudioStopCommand();
            SearchCommand = new SearchCommand();
            SupportCommand = new SupportCommand();
            MainPlayState_Changed(null, null);
            //BackgroundAudioPlayer.Instance.PlayStateChanged += MainPlayState_Changed;

            TileSize = (Window.Current.Bounds.Width - (19 * 2) - (12 * 2)) / 2;
        }
        
        public void NavigateTo() {
            ((Frame) Window.Current.Content).Navigate(typeof(MainPage));
        }

        public void CheckDataLoaded()
        {
            if (!IsTipsLoading && !IsNieuwLoading && !IsPopulairLoading && !IsIndexLoading && !IsLiveLoading) IsDataLoaded = true;
        }

        public async void LoadTipsAsync()
        {
            IsDataLoaded = false;
            IsTipsLoading = true;
            Tips = await UitzendingGemistService.GetTips();
            IsTipsLoading = false;
            CheckDataLoaded();
        }

        public async void LoadNieuwAsync()
        {
            IsDataLoaded = false;
            IsNieuwLoading = true;
            New = await UitzendingGemistService.GetRecent();
            IsNieuwLoading = false;
            CheckDataLoaded();
        }

        public async void LoadPopulairAsync() {
            IsDataLoaded = false;
            IsPopulairLoading = true;
            Popular = await UitzendingGemistService.GetPopularEpisodes();
            IsPopulairLoading = false;
            CheckDataLoaded();
        }

        public async void LoadIndexAsync()
        {
            IsDataLoaded = false;
            IsIndexLoading = true;
            List<Series> SeriesList = await UitzendingGemistService.GetSeriesIndex();
            foreach (Series s in SeriesList) s.Name = s.Name.Trim();
            Index = JumpListHelper.ToAlphaGroups(SeriesList, s => s.Name);
            IsIndexLoading = false;
            CheckDataLoaded();
        }

        public async void LoadLiveAsync()
        {
            IsDataLoaded = false;
            IsLiveLoading = true;
            Live = await TvnlService.GetLiveItems();
            IsLiveLoading = false;
            CheckDataLoaded();
        }

        public async void LoadData()
        {
            if (!RadioBox2Service.CheckConnection())
            {
                bool result = await MessageBox.Show("Er kon geen contact gemaakt worden met internet, controleer je Wi-Fi of dataverbinding.", "Geen internetverbinding");
                if (result)
                {
                    Terminate();
                }
            }
            else
            {
                IsDataLoaded = false;

                LoadTipsAsync();
                LoadNieuwAsync();
                LoadPopulairAsync();
                LoadIndexAsync();
                LoadLiveAsync();
            }
        }

        private void MainPlayState_Changed(object sender, EventArgs e)
        {            
            /*switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    AppBarPlayButtonText = "Pauzeer";
                    AppBarPlayButtonIcon = new Uri("/Resources/appbar.control.pause.png", UriKind.Relative);
                    HasAudio = 1;
                    break;
                case PlayState.Paused:
                    AppBarPlayButtonText = "Afspelen";
                    AppBarPlayButtonIcon = new Uri("/Resources/appbar.control.play.png", UriKind.Relative);
                    HasAudio = 1;
                    break;
                case PlayState.Stopped:
                    HasAudio = 0;
                    break;
                default:
                    HasAudio = 0;
                    break;
            }*/
        }

        private static void Terminate()
        {
            App.Current.Exit();
        }
    }
}
