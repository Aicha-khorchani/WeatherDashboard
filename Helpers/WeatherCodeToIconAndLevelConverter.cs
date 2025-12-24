using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WeatherDashboard.Helpers
{
    public class WeatherCodeToIconAndLevelConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            if (!int.TryParse(value.ToString(), out int code))
                return null;

            string iconFile = "1.png";
            string levelText = "";

            // Map code to icon + level
            switch (code)
            {
                // Sunny / Clear
                case 0:
                case 1:
                case 2:
                    iconFile = "4.png";   // Sunny
                    levelText = "Clear";
                    break;

                // Overcast
                case 3:
                    iconFile = "1.png";   // Overcast
                    levelText = "Overcast";
                    break;

                // Fog / Windy
                case 45:
                case 48:
                    iconFile = "6.png";
                    levelText = "Foggy";
                    break;

                // Drizzle / Rain
                case 51:
                case 53:
                case 55:
                case 61:
                case 63:
                case 65:
                case 80:
                case 81:
                case 82:
                    iconFile = "2.png";
                    levelText = code switch
                    {
                        51 or 61 or 80 => "Light",
                        53 or 63 or 81 => "Moderate",
                        55 or 65 or 82 => "Heavy",
                        _ => ""
                    };
                    break;

                // Snow
                case 71:
                case 73:
                case 75:
                    iconFile = "3.png";
                    levelText = code switch
                    {
                        71 => "Light",
                        73 => "Moderate",
                        75 => "Heavy",
                        _ => ""
                    };
                    break;

                // Thunder
                case 95:
                case 96:
                case 99:
                    iconFile = "5.png";
                    levelText = code switch
                    {
                        95 => "Thunderstorm",
                        96 => "Slight hail",
                        99 => "Heavy hail",
                        _ => ""
                    };
                    break;

                default:
                    iconFile = "1.png";
                    levelText = "Unknown";
                    break;
            }

            if (parameter?.ToString() == "Level")
                return levelText;

            // Default: return image
            string path = $"/Assets/Icons/{iconFile}";
            try
            {
                return new BitmapImage(new Uri($"pack://application:,,,{path}"));
            }
            catch
            {
                return new BitmapImage(new Uri($"pack://application:,,,/Assets/Icons/1.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
