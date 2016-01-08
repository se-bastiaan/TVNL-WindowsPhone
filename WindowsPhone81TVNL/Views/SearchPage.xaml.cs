using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.ViewModels;


namespace WindowsPhone81TVNL.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        public SearchPage()
        {
            this.InitializeComponent();

            DataContext = ViewModelLocator.Search;
            ViewModelLocator.Search.PropertyChanged += OnPropertyChanged;
            PivotFadeOut(false);
            SearchingFadeOut(false);
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

        private async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsDataLoaded"))
            {
                if (ViewModelLocator.Search.IsDataLoaded)
                {
                    await Task.Run(() => { new System.Threading.ManualResetEvent(false).WaitOne(500); });
                    SearchingFadeOut(true);
                }
                else if (ViewModelLocator.Search.FirstLoad)
                {
                    ViewModelLocator.Search.FirstLoad = false;
                    SearchingFadeIn();
                }
                else
                {
                    PivotFadeOut(true);
                }
            }
            else if (e.PropertyName.Equals("Series"))
            {
                if (ViewModelLocator.Search.Series != null && ViewModelLocator.Search.Series.Count > 0)
                {
                    EmptySeries.Visibility = Visibility.Collapsed;
                    SeriesList.Visibility = Visibility.Visible;
                }
                else
                {
                    EmptySeries.Visibility = Visibility.Visible;
                    SeriesList.Visibility = Visibility.Collapsed;
                }
            }
            else if (e.PropertyName.Equals("Episodes"))
            {
                if (ViewModelLocator.Search.Episodes != null && ViewModelLocator.Search.Episodes.Count > 0)
                {
                    EmptyEpisodes.Visibility = Visibility.Collapsed;
                    EpisodesList.Visibility = Visibility.Visible;
                }
                else
                {
                    EmptyEpisodes.Visibility = Visibility.Visible;
                    EpisodesList.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SearchBoxKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModelLocator.Search.Search(SearchBox.Text);
                //FlurrySDK.LogEvent("DoSearch");
            }
        }

        private void SearchBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (TypeToSearchPanel.Visibility != Visibility.Collapsed)
                TypeToSearchFadeOut();
        }

        private void EpisodesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EpisodesList.SelectedItem == null)
                return;

            //FlurrySDK.LogEvent("OpenEpisodeFromSearch");

            Episode item = (Episode)EpisodesList.SelectedItem;
            EpisodeDetailViewModel.NavigateTo(item);

            EpisodesList.SelectedItem = null;
        }

        private void SeriesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SeriesList.SelectedItem == null)
                return;

            //FlurrySDK.LogEvent("OpenSeriesFromSearch");

            Series item = (Series)SeriesList.SelectedItem;
            EpisodeDetailViewModel.NavigateTo(item);

            SeriesList.SelectedItem = null;
        }

        private void TypeToSearchFadeOut()
        {
            Storyboard sb = new Storyboard() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            FadeOutThemeAnimation fadeAnim = new FadeOutThemeAnimation() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
            Storyboard.SetTarget(fadeAnim, TypeToSearchPanel);
            sb.Children.Add(fadeAnim);

            DoubleAnimation doubleAnim = new DoubleAnimation() { To = 20, From = 0, Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
            TranslateTransform translate = new TranslateTransform();
            doubleAnim.EnableDependentAnimation = true;
            TypeToSearchPanel.RenderTransform = translate;
            Storyboard.SetTarget(doubleAnim, translate);
            Storyboard.SetTargetProperty(doubleAnim, "Y");
            sb.Children.Add(doubleAnim);

            sb.Completed += delegate
            {
                TypeToSearchPanel.Visibility = Visibility.Collapsed;
                sb.Stop();
            };
            sb.Begin();
        }

        private void PivotFadeOut(bool searchFadeIn)
        {
            Storyboard sb = new Storyboard() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            FadeOutThemeAnimation fadeAnim = new FadeOutThemeAnimation() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
            Storyboard.SetTarget(fadeAnim, PivotControl);
            sb.Children.Add(fadeAnim);

            sb.Completed += delegate
            {
                PivotControl.Visibility = Visibility.Collapsed;
                sb.Stop();
                if (searchFadeIn) SearchingFadeIn();
            };
            sb.Begin();
        }

        private void SearchingFadeOut(bool pivotFadeIn)
        {
            Storyboard sb = new Storyboard() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            FadeOutThemeAnimation fadeAnim = new FadeOutThemeAnimation() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
            Storyboard.SetTarget(fadeAnim, SearchingPanel);
            sb.Children.Add(fadeAnim);
            
            sb.Completed += delegate
            {
                SearchingPanel.Visibility = Visibility.Collapsed;
                sb.Stop();
                if (pivotFadeIn) PivotFadeIn();
            };
            sb.Begin();
        }

        private void PivotFadeIn()
        {
            PivotControl.Visibility = Visibility.Visible;
            Storyboard sb = new Storyboard() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            FadeInThemeAnimation fadeAnim = new FadeInThemeAnimation() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
            Storyboard.SetTarget(fadeAnim, PivotControl);
            sb.Children.Add(fadeAnim);
            
            sb.Completed += delegate
            {
                sb.Stop();
            };
            sb.Begin();
        }

        private void SearchingFadeIn()
        {
            SearchingPanel.Visibility = Visibility.Visible;
            Storyboard sb = new Storyboard() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };

            FadeInThemeAnimation fadeAnim = new FadeInThemeAnimation() { Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
            Storyboard.SetTarget(fadeAnim, SearchingPanel);
            sb.Children.Add(fadeAnim);

            sb.Completed += delegate
            {
                sb.Stop();
            };
            sb.Begin();
        }
    }
}