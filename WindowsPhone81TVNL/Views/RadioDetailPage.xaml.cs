using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.ViewModels;

namespace WindowsPhone81TVNL.Views
{

    public sealed partial class RadioDetailPage : Page
    {
        public RadioDetailPage()
        {
            this.InitializeComponent();

            ViewModelLocator.RadioDetail = new RadioDetailViewModel();
            DataContext = ViewModelLocator.RadioDetail;

            Loaded += RadioDetailPage_Loaded;

            BackgroundMediaPlayer.Current.CurrentStateChanged += BackgroundMediaPlayerCurrentStateChanged;
        }

        void RadioDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainGrid.Margin = new Thickness(0, 0, 0, CommandBar.ActualHeight);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModelLocator.RadioDetail.Item = e.Parameter as LiveItem;
            ViewModelLocator.RadioDetail.LoadData();

            if (ViewModelLocator.RadioDetail.Item.WebcamUrl == "")
                WebcamButton.Visibility = Visibility.Collapsed;

            StatusBar.GetForCurrentView().BackgroundOpacity = 0;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            Rect rect = StatusBar.GetForCurrentView().OccludedRect;
            Logo.Margin = new Thickness(0, rect.Height + 16, 0, 0);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            StatusBar.GetForCurrentView().BackgroundOpacity = 1;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseVisible);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        private void PlayWebcam(object sender, RoutedEventArgs e)
        {
            StreamPage.NavigateTo(new StreamObject(ViewModelLocator.RadioDetail.Item.WebcamUrl, true, true));
        }

        private void PlayAudio(object sender, RoutedEventArgs e)
        {
            if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
            {
                AudioButton.Icon = new SymbolIcon(Symbol.Play);
                AudioButton.Label = "luisteren";
            }
            else
            {
                AudioButton.Icon = new SymbolIcon(Symbol.Pause);
                AudioButton.Label = "pauzeren";
            }
            ViewModelLocator.RadioDetail.PlayAudio();
        }

        private async void BackgroundMediaPlayerCurrentStateChanged(MediaPlayer sender, object args)
        {
            try
            {
                if (sender.CurrentState == MediaPlayerState.Playing)
                {
                    await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        AudioButton.Icon = new SymbolIcon(Symbol.Pause);
                        AudioButton.Label = "pauzeren";
                    });
                }
                else if (sender.CurrentState == MediaPlayerState.Paused)
                {
                    await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        AudioButton.Icon = new SymbolIcon(Symbol.Play);
                        AudioButton.Label = "luisteren";
                    });
                }
            }
            catch (Exception e) { }
        }

    }
}
