using System.Collections.Generic;
using WeatherDashboard.Models;

namespace WeatherDashboard.ViewModels
{
    public class WeeklyForecastViewModel
    {
        public List<DailyForecast> DailyForecasts { get; set; } = new();

        // Constructor can accept the list from MainViewModel
        public WeeklyForecastViewModel(List<DailyForecast> forecasts)
        {
            if (forecasts != null)
                DailyForecasts = forecasts;
        }

        // Default constructor (for designer)
        public WeeklyForecastViewModel() { }
    }
}
