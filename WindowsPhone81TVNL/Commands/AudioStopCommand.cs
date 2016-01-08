using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WindowsPhone81TVNL.Commands
{
    public class AudioStopCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public void Execute(object parameter)
        {
            /*if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing || BackgroundAudioPlayer.Instance.PlayerState == PlayState.Paused)
            {
                BackgroundAudioPlayer.Instance.Stop();
            }*/
        }
    }
}
