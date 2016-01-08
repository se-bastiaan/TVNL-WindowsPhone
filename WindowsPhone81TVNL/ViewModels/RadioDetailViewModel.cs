using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.Models;
using WindowsPhone81TVNL.Services;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.ViewModels
{
    public class RadioDetailViewModel : BaseViewModel
    {
        private LiveItem _item;
        public LiveItem Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private List<Programme> _programme;
        public List<Programme> Programme
        {
            get { return _programme; }
            set { SetProperty(ref _programme, value); }
        }

        private NowOnAir _onAir;
        public NowOnAir OnAir
        {
            get { return _onAir; }
            set { SetProperty(ref _onAir, value); }
        }

        private Broadcast _broadcast;
        public Broadcast Broadcast
        {
            get { return _broadcast; }
            set { SetProperty(ref _broadcast, value); }
        }

        private DispatcherTimer _broadcastRefreshTimer, _nowOnAirRefreshTimer;
        private MediaPlayer _mediaPlayer;

        public RadioDetailViewModel()
        {
            _mediaPlayer = BackgroundMediaPlayer.Current;
        }

        public async void LoadData()
        {
            IsDataLoaded = false;

            OnAir = await RadioBox2Service.GetNowOnAir(Item.Channel.ToString());
            Broadcast = await RadioBox2Service.GetCurrentBroadcast(Item.Channel.ToString());

            if (_nowOnAirRefreshTimer == null)
            {
                _nowOnAirRefreshTimer = new DispatcherTimer();
                _nowOnAirRefreshTimer.Interval = DateTime.Now.AddMilliseconds(30000) - DateTime.Now;
                _nowOnAirRefreshTimer.Tick += new EventHandler<object>(LoadOnAir);
                _nowOnAirRefreshTimer.Start();
            }

            if (_broadcastRefreshTimer == null)
            {
                _broadcastRefreshTimer = new DispatcherTimer();
                _broadcastRefreshTimer.Interval = DateTime.Now.AddMilliseconds(120000) - DateTime.Now;
                _broadcastRefreshTimer.Tick += new EventHandler<object>(LoadBroadcast);
                _broadcastRefreshTimer.Start();
            }

            LoadProgramme();
            IsDataLoaded = true;
        }

        public async void LoadProgramme()
        {
            Programme = await RadioBox2Service.getNextProgrammes(Item.Channel.ToString());
        }

        public async void LoadBroadcast(object sender, object e)
        {
            if (_broadcastRefreshTimer != null && _broadcastRefreshTimer.IsEnabled)
            {
                _broadcastRefreshTimer.Stop();
                _broadcastRefreshTimer = null;
            }

            int Id = 0;
            if (Broadcast != null)
            {
                Id = Broadcast.Id;
            }
            Broadcast = await RadioBox2Service.GetCurrentBroadcast(Item.Channel.ToString());

            if (Id != Broadcast.Id) LoadProgramme();

            _broadcastRefreshTimer = new DispatcherTimer();
            _broadcastRefreshTimer.Interval = DateTime.Now.AddMilliseconds(120000) - DateTime.Now;
            _broadcastRefreshTimer.Tick += new EventHandler<object>(LoadBroadcast);
            _broadcastRefreshTimer.Start();
        }

        public async void LoadOnAir(object sender, object e)
        {
            if (_nowOnAirRefreshTimer != null && _nowOnAirRefreshTimer.IsEnabled)
            {
                _nowOnAirRefreshTimer.Stop();
                _nowOnAirRefreshTimer = null;
            }

            OnAir = await RadioBox2Service.GetNowOnAir(Item.Channel.ToString());

            _nowOnAirRefreshTimer = new DispatcherTimer();
            _nowOnAirRefreshTimer.Interval = DateTime.Now.AddMilliseconds(30000) - DateTime.Now;
            _nowOnAirRefreshTimer.Tick += new EventHandler<object>(LoadOnAir);
            _nowOnAirRefreshTimer.Start();
        }

        public static void NavigateTo(LiveItem item)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(RadioDetailPage), item);
        }

        public void PlayAudio()
        {   
            var message = new ValueSet();
            if (BackgroundMediaPlayer.Current.CurrentState != MediaPlayerState.Playing)
            {
                message = new ValueSet
                    {
                        {
                            "Play",
                            (SettingsHelper.ReadSettingsValue<bool>("hq", false) ? Item.Url : Item.HqUrl)
                        },
                        {
                            "Title",
                            Item.Name
                        },
                        {
                            "Artist",
                            "NPO"
                        }
                    };
            }
            else
            {
                message = new ValueSet
                    {
                        {
                            "Pause",
                            "1"
                        }
                    };
            }
            BackgroundMediaPlayer.SendMessageToBackground(message);
        }
    }
}
