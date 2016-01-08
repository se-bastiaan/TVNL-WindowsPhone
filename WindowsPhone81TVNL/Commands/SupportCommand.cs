using System;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsPhone81TVNL.Helpers;
using WindowsPhone81TVNL.ViewModels;
using WindowsPhone81TVNL.Views;

namespace WindowsPhone81TVNL.Commands
{
    public class SupportCommand : ICommand
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
            try
            {
                ((Frame)Window.Current.Content).Navigate(typeof(SupportPage));
            }
            catch (Exception e)
            {
#if DEBUG
                MessageBox.Show(ResourceHelper.GetString("ErrorTitle"), e.ToString());
#else
                MessageBox.Show(ResourceHelper.GetString("ErrorTitle"), e.Message);
#endif
            }
        }
    }
}
