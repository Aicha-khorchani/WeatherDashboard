using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WeatherDashboard.Helpers
{
    // converts between weather card and welcom mgs at app startup 
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = value == null;

            // If parameter is "Invert", reverse the logic
            if ((parameter as string)?.ToLower() == "invert")
                isNull = !isNull;

            return isNull ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
