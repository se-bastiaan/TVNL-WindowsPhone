using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace WindowsPhone81TVNL.Converters
{
    public class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String culture)
        {
            return !(bool)value;
        }
    }
}
