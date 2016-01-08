using WindowsPhone81TVNL.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.Services;
using Microsoft.PlayerFramework;
using Windows.Phone.UI.Input;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WindowsPhone81TVNL.Views
{

    public sealed partial class StreamPage : Page
    {
        public StreamPage()
        {
            this.InitializeComponent();
        }

        public static void NavigateTo(StreamObject item)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(StreamPage), item);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var streamData = e.Parameter as StreamObject;
            InitPlayer(streamData.Id, streamData.IsLive, streamData.IsWebcam);

            await StatusBar.GetForCurrentView().HideAsync();
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            await StatusBar.GetForCurrentView().ShowAsync();
            StreamPlayer.Dispose();
        }

        public async void InitPlayer(string id, bool live = false, bool webcam = false)
        {
            UriShieldData Data;
            if (webcam)
            {
                Data = await StreamService.GetVideoStream(id);
            }
            else
            {
                Data = await OdiService.getUriShieldData(id, live);
            }

            if (Data != null)
            {
                if (live)
                {
                    StreamPlayer.IsLive = true;
                    StreamPlayer.IsInfoVisible = false;
                    StreamPlayer.IsTimelineVisible = true;
                    StreamPlayer.IsDurationVisible = false;
                    StreamPlayer.IsTimeRemainingVisible = false;
                    StreamPlayer.IsTimeElapsedVisible = false;
                }
                else
                {
                    UitzendingGemistService.PostView(id);
                }

                StreamPlayer.AutoPlay = true;
                StreamPlayer.Source = new Uri(Data.Url, UriKind.Absolute);
                StreamPlayer.IsResolutionIndicatorVisible = false;
                StreamPlayer.IsAudioSelectionVisible = false;
                StreamPlayer.IsVolumeVisible = false;
                StreamPlayer.Volume = 1;

                if (Data.Subtitles && !live)
                {
                    Microsoft.PlayerFramework.Caption caption = new Microsoft.PlayerFramework.Caption();

                    StreamPlayer.IsCaptionSelectionVisible = true;
                    caption.Source = new Uri(UitzendingGemistService.GetVTTUrl(id), UriKind.Absolute);
                    caption.Description = "Nederlands";
                    StreamPlayer.AvailableCaptions.Clear();
                    StreamPlayer.AvailableCaptions.Add(caption);
                }
                else
                {
                    StreamPlayer.IsCaptionSelectionVisible = false;
                }
            }

            RingProgress.Visibility = Visibility.Collapsed;
        }
    }
}
