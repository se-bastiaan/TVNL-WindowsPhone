using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WindowsPhone81TVNL.Views
{
    public sealed partial class MainPage : Page
    {
        bool ListGenerated = false;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            DataContext = ViewModelLocator.Main;
            ListGenerated = false;  
            ViewModelLocator.Main.LoadData();    
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;      
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        private void MainPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewBase list = sender as ListViewBase;

            if (list == null || list.SelectedIndex == -1) return;

            Frame currentFrame = (Window.Current.Content as Frame);

            if (list.ItemsSource == ViewModelLocator.Main.Tips)
            {
                TipItem item = list.SelectedItem as TipItem;
                if(item != null && item.Episode != null)
                    EpisodeDetailViewModel.NavigateTo(item.Episode);
            }
            else if (list.ItemsSource == ViewModelLocator.Main.New)
            {
                RecentItem item = list.SelectedItem as RecentItem;
                if (item != null && item.Episode != null)
                    EpisodeDetailViewModel.NavigateTo(item.Episode);
            }
            else if (list.ItemsSource == ViewModelLocator.Main.Popular)
            {
                Episode item = list.SelectedItem as Episode;
                if (item != null)
                    EpisodeDetailViewModel.NavigateTo(item);
            }
            else if (list.ItemsSource == ViewModelLocator.Main.Live)
            {
                LiveItem item = list.SelectedItem as LiveItem;
                if (item != null && !item.IsRadio && item.Guide.Count > 0)
                {
                    LiveDetailViewModel.NavigateTo(item);
                }
                else if(item != null && !item.IsRadio && item.Guide.Count == 0)
                {
                    StreamPage.NavigateTo(new StreamObject(item.Url, true));
                }
                else if(item != null && item.IsRadio)
                {
                    RadioDetailViewModel.NavigateTo(item);
                }
            }

            list.SelectedIndex = -1;
        }

        private void MainPage_IndexListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewBase list = sender as ListViewBase;

            if (!ViewModelLocator.Main.IsDataLoaded || !ListGenerated)
            {
                ListGenerated = true;
                list.SelectedIndex = -1;
                return;
            }

            if (list == null || list.SelectedIndex == -1) return;

            Series item = list.SelectedItem as Series;
            if (item != null)
                EpisodeDetailViewModel.NavigateTo(item);

            list.SelectedIndex = -1;
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Main.LoadData();
        }
    }
}
