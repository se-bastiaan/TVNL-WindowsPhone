using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPhone81TVNL.Helpers;

namespace WindowsPhone81TVNL.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        protected bool _isDataLoaded = true;
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set {
                if (value)
                {
                    StatusBarHelper.HideProgressIndicator();
                } else {
                    StatusBarHelper.ShowProgressIndicator("Gegevens ophalen...");
                }
                SetProperty(ref _isDataLoaded, value);
            }
        }

        public void NavigateTo()
        {
            
        }
    }
}
