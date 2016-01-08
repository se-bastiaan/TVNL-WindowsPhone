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
using WindowsPhone81TVNL.ViewModels;

namespace WindowsPhone81TVNL.Views
{

    public sealed partial class LiveDetailPage : Page
    {
        public LiveDetailPage()
        {
            this.InitializeComponent();

            ViewModelLocator.LiveDetail = new LiveDetailViewModel();
            DataContext = ViewModelLocator.LiveDetail;
            Loaded += LiveDetailPage_Loaded;
        }

        void LiveDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainGrid.Margin = new Thickness(0, 0, 0, CommandBar.ActualHeight);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LiveItem item = e.Parameter as LiveItem;
            ViewModelLocator.LiveDetail.LoadData(item);

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

        private void PlayButton(object sender, RoutedEventArgs e)
        {
            StreamPage.NavigateTo(new StreamObject(ViewModelLocator.LiveDetail.Item.Url, true));
        }

    }
}
