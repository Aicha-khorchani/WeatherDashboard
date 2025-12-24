using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherDashboard.Models;
using WeatherDashboard.Helpers;
using System.Globalization;

namespace WeatherDashboard.Services
{
    public class WeatherApiService
    {
        private readonly HttpClient _httpClient;

        public WeatherApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast/")
            };
        }

        /// <summary>
        /// Fetch weather data for a city (current, daily, hourly)
        /// </summary>
        public async Task<(CurrentWeather? current, List<DailyForecast>? daily)> GetWeatherAsync(City city)
        {
            if (city == null)
            {
                ToastNotifier.ShowError("City cannot be null");
                return (null, null);
            }

            try
            {
                // Open-Meteo API: current + daily + hourly
                string url =
                    $"?latitude={city.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={city.Longitude.ToString(CultureInfo.InvariantCulture)}" +
                    "&current_weather=true" +
                    "&daily=temperature_2m_max,temperature_2m_min,windspeed_10m_max,weathercode" +
                    "&timezone=auto";

                Logger.Log($"Weather API URL: {url}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Log($"Weather API error: {response.StatusCode}");
                    ToastNotifier.ShowError("Failed to fetch weather data");
                    return (null, null);
                }

                var json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);

                // Current weather
                CurrentWeather? current = null;
                if (doc.RootElement.TryGetProperty("current_weather", out JsonElement currentElem))
                {
                    current = new CurrentWeather
                    {
                        Temperature = currentElem.GetProperty("temperature").GetDouble(),
                        WindSpeed = currentElem.GetProperty("windspeed").GetDouble(),
                        WindDirection = currentElem.GetProperty("winddirection").GetInt32(),
                        WeatherCode = currentElem.GetProperty("weathercode").GetInt32().ToString(),
                        Condition = currentElem.GetProperty("weathercode").GetInt32().ToString(),
                        Humidity = 0 // Open-Meteo current_weather does NOT return humidity unfortantly so i will see i may get it from another appi or find solution to get it 
                    };

                }

                // Daily forecast
                List<DailyForecast> dailyForecasts = new();
                if (doc.RootElement.TryGetProperty("daily", out JsonElement dailyElem))
                {
                    var dates = dailyElem.GetProperty("time").EnumerateArray();
                    var tempMax = dailyElem.GetProperty("temperature_2m_max").EnumerateArray();
                    var tempMin = dailyElem.GetProperty("temperature_2m_min").EnumerateArray();
                    var windMax = dailyElem.GetProperty("windspeed_10m_max").EnumerateArray();
                    var codes = dailyElem.GetProperty("weathercode").EnumerateArray();

                    while (dates.MoveNext() && tempMax.MoveNext() && tempMin.MoveNext() &&
                           windMax.MoveNext() && codes.MoveNext())
                    {
                        dailyForecasts.Add(new DailyForecast
                        {
                            Date = DateTime.Parse(dates.Current.GetString() ?? DateTime.Now.ToString()),
                            MaxTemperature = tempMax.Current.GetDouble(),
                            MinTemperature = tempMin.Current.GetDouble(),
                            AverageHumidity = 0,
                            WindSpeed = windMax.Current.GetDouble(),
                            WeatherCode = codes.Current.GetInt32().ToString(),
                            Condition = codes.Current.GetInt32().ToString()
                        });
                    }
                }

                Logger.Log($"Weather fetched for {city.Name}");
                return (current, dailyForecasts);
            }
            catch (HttpRequestException)
            {
                ToastNotifier.ShowError("No internet connection");
                Logger.Log("Weather API request failed: No internet");
            }
            catch (Exception ex)
            {
                ToastNotifier.ShowError("Error fetching weather data");
                Logger.Log(ex, "Unexpected error in WeatherApiService");
            }

            return (null, null);
        }
    }
}
