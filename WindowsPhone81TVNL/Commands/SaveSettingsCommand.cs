using System;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WindowsPhone81TVNL.Helpers;

namespace WindowsPhone81TVNL.Commands
{
    public class SaveSettingsCommand : ICommand
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
                // Make sure the binding updates when TextBox has focus
                var focusObj = FocusManager.GetFocusedElement();
                if (focusObj != null && focusObj is TextBox)
                {
                    var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                    binding.UpdateSource();
                }

                var rootVisual = (Frame)Window.Current.Content;
                if (rootVisual.CanGoBack)
                    rootVisual.GoBack();
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
