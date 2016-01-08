﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WindowsPhone81TVNL.Helpers
{
    public class NavigationHelper
    {
        readonly Frame frame;

        public NavigationHelper(Frame frame)
        {
            this.frame = frame;
        }

        public void GoBack()
        {
            frame.GoBack();
        }

        public void GoForward()
        {
            frame.GoForward();
        }

        public bool Navigate<T>(object parameter = null)
        {
            var type = typeof(T);

            return Navigate(type, parameter);
        }

        public bool Navigate(Type source, object parameter = null)
        {
            return frame.Navigate(source, parameter);
        }
    }
}