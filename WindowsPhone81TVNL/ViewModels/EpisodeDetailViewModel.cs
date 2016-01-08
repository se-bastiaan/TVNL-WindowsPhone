using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.Services;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.ViewModels
{
    public class EpisodeDetailViewModel : BaseViewModel
    {
        public string WhatsonId { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _image;
        public string Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _broadcasters;
        public string Broadcasters
        {
            get { return _broadcasters; }
            set { SetProperty(ref _broadcasters, value); }
        }

        private string _genres;
        public string Genres
        {
            get { return _genres; }
            set { SetProperty(ref _genres, value); }
        }

        private Episode _episode;
        public Episode Episode
        {
            get { return _episode; }
            set { SetProperty(ref _episode, value); }
        }

        private List<Episode> _episodes;
        public List<Episode> Episodes
        {
            get { return _episodes; }
            set { SetProperty(ref _episodes, value); }
        }

        private bool _noEpisodes;
        public bool NoEpisodes
        {
            get { return _noEpisodes; }
            set { SetProperty(ref _noEpisodes, value); }
        }

        private List<string> _images;
        public List<string> Images
        {
            get { return _images; }
            set { SetProperty(ref _images, value); }
        }

        public async void LoadData(Boolean series, String Id)
        {
            IsDataLoaded = false;
            Episode episodeItem = null;

            Image = "";

            if (series)
            {
                episodeItem = await UitzendingGemistService.GetSeriesLatestEpisode(Id);
            }
            else
            {
                episodeItem = await UitzendingGemistService.GetEpisode(Id);
            }

            if (episodeItem != null)
            {
                WhatsonId = episodeItem.WhatsonId;
                Name = episodeItem.Series.Name;
                Image = "";
                if (episodeItem.Stills.Count > 0)
                    Image = episodeItem.Stills[0].Url;
                Description = episodeItem.Description;
                if (Description.Equals("")) Description = "Geen beschrijving gevonden.";
                Images = new List<string>();
                Broadcasters = "";
                foreach (string broadcaster in episodeItem.Broadcasters)
                {
                    if (Broadcasters.Length > 0) Broadcasters += ", ";
                    Broadcasters += broadcaster;
                }
                string genres = "";
                foreach (string genre in episodeItem.Genres)
                {
                    if (genres.Length > 0) genres += ", ";
                    genres += genre;
                }
                Genres = genres;
                foreach (string advisory in episodeItem.Advisories)
                {
                    Images.Add("/Resources/Kijkwijzer/" + advisory + ".png");
                }
                Episode = episodeItem;
                try
                {
                    Series seriesData = await UitzendingGemistService.GetSeries(episodeItem.Series.Id);
                    Episodes = new List<Episode>();
                    int max = 20;
                    if (seriesData.Episodes.Count < 50) max = seriesData.Episodes.Count;
                    for (int i = 0; i < max; i++)
                    {
                        Episode episode = seriesData.Episodes[i];
                        episode.Series = episodeItem.Series;
                        if (!episodeItem.WhatsonId.Equals(episode.WhatsonId) && episode != null)
                        {
                            Episodes.Add(episode);
                        }
                        else
                        {
                            max++;
                        }
                    }
                    if (Episodes.Count == 0) NoEpisodes = true;
                }
                catch (Exception e)
                {
                }
            }
            IsDataLoaded = true;
        }

        public static void NavigateTo(Episode e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(EpisodeDetailPage), e);
        }

        public static void NavigateTo(Series s)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(EpisodeDetailPage), s);
        }
    }
}
