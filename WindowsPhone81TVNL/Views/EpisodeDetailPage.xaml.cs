using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public sealed partial class EpisodeDetailPage : Page
    {
        public EpisodeDetailPage()
        {
            this.InitializeComponent();
            ViewModelLocator.EpisodeDetail = new EpisodeDetailViewModel();
            DataContext = ViewModelLocator.EpisodeDetail;
            Loaded += EpisodeDetailPage_Loaded;
        }

        void EpisodeDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainGrid.Margin = new Thickness(0, 0, 0, CommandBar.ActualHeight);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameter = e.Parameter;

            if (parameter.GetType() == typeof(Episode))
            {
                ViewModelLocator.EpisodeDetail.LoadData(false, (parameter as Episode).WhatsonId);
            }
            else if (parameter.GetType() == typeof(Series))
            {
                ViewModelLocator.EpisodeDetail.LoadData(true, (parameter as Series).NeboId);
            }

            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            StatusBar.GetForCurrentView().BackgroundOpacity = 0;
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            Rect rect = StatusBar.GetForCurrentView().OccludedRect;
            Logo.Margin = new Thickness(0, rect.Height + 16, 0, 0);
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
            StreamPage.NavigateTo(new StreamObject(ViewModelLocator.EpisodeDetail.WhatsonId));
        }

        private void EpisodesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewBase list = sender as ListViewBase;

            if (list == null || list.SelectedIndex == -1) return;

            Episode item = list.SelectedItem as Episode;
            if(item != null)
                EpisodeDetailViewModel.NavigateTo(item);

            list.SelectedIndex = -1;
        }
    }
}
