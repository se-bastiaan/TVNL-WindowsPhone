using System;
using System.Windows.Input;

namespace WindowsPhone81TVNL.Commands
{
    public class ReviewCommand : ICommand
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
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=32975086-27ce-463e-8bce-a46e3a1026b5"));
        }
    }
}
