using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WindowsPhone81TVNL.Helpers;

namespace WindowsPhone81TVNL.Commands
{
    public class ContactCommand : ICommand
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

        public async void Execute(object parameter)
        {
            var mailto = new Uri("mailto:?to=" + ResourceHelper.GetString("ContactTo") + "&subject=" + ResourceHelper.GetString("ContactSubject"));
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }
    }
}
