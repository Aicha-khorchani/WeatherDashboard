using System;

namespace WeatherDashboard.Models
{
    public class HourlyWeatherPoint
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }   // °C
        public double Humidity { get; set; }      // %
        public double WindSpeed { get; set; }     // km/h
        public string Condition { get; set; } = string.Empty; // optional
        public string WeatherCode { get; set; } = string.Empty; // for icon mapping
    }
}
