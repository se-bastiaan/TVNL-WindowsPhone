using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace WindowsPhone81TVNL.Converters
{
    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, String str)
        {
            if (value == null) return null;

            if (value is string)
            {
                var caseParam = parameter as string;
                value = ((string)value).ToLower();

                if (caseParam != null && caseParam.ToLower() == "u")
                    return ((string)value).ToUpper();
                if (caseParam != null && caseParam.ToLower() == "t")
                    return ((string)value).Substring(0, 1).ToUpper() + ((string)value).Substring(1);
                return value;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, String str)
        {
            return value;
        }
    }
}
