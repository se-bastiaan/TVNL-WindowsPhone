
namespace WindowsPhone81TVNL.ViewModels
{
    public class ViewModelLocator
    {
        private static MainViewModel _main;
        public static MainViewModel Main
        {
            get { return _main ?? (_main = new MainViewModel()); }
        }

        private static SearchViewModel _search;
        public static SearchViewModel Search
        {
            get { return _search ?? (_search = new SearchViewModel()); }
        }

        private static AboutViewModel _about;
        public static AboutViewModel About
        {
            get { return _about ?? (_about = new AboutViewModel()); }
        }

        private static EpisodeDetailViewModel _episode_detail;
        public static EpisodeDetailViewModel EpisodeDetail
        {
            get { return _episode_detail ?? (_episode_detail = new EpisodeDetailViewModel()); }
            set { _episode_detail = value; }
        }

        private static LiveDetailViewModel _live_detail;
        public static LiveDetailViewModel LiveDetail
        {
            get { return _live_detail ?? (_live_detail = new LiveDetailViewModel()); }
            set { _live_detail = value; }
        }

        private static RadioDetailViewModel _radio_detail;
        public static RadioDetailViewModel RadioDetail
        {
            get { return _radio_detail ?? (_radio_detail = new RadioDetailViewModel()); }
            set { _radio_detail = value; }
        }
    }
}
