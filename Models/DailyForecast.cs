using System;
using System.Collections.Generic;

namespace WeatherDashboard.Models
{
    public class DailyForecast
    {
        public DateTime Date { get; set; }
        public double MinTemperature { get; set; }   // °C
        public double MaxTemperature { get; set; }   // °C
        public double AverageHumidity { get; set; }  // %
        public double WindSpeed { get; set; }        // km/h
        public double Precipitation { get; set; }
        
        public double FeelsLikeMax { get; set; }
        public double FeelsLikeMin { get; set; }
        public string Condition { get; set; } = string.Empty; // e.g., Rain, Sunny
        public string WeatherCode { get; set; } = string.Empty; // Icon mapping
        public List<HourlyWeatherPoint> HourlyPoints { get; set; } = new();
          public double UvIndexMax { get; set; }

        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
    }
}
