using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherDashboard.Models;
using WeatherDashboard.Helpers;

namespace WeatherDashboard.Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://geocoding-api.open-meteo.com/v1/")
            };
        }

        /// <summary>
        /// Get City info (latitude, longitude) from city name
        /// </summary>
        public async Task<City?> GetCityAsync(string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                ToastNotifier.ShowError("City name cannot be empty");
                return null;
            }

            try
            {
                var response = await _httpClient.GetAsync($"search?name={Uri.EscapeDataString(cityName)}&count=1");
                
                if (!response.IsSuccessStatusCode)
                {
                    Logger.Log($"Geocoding API error: {response.StatusCode}");
                    ToastNotifier.ShowError("Failed to fetch city info from API");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);

                var results = doc.RootElement.GetProperty("results");
                if (results.GetArrayLength() == 0)
                {
                    ToastNotifier.ShowError("City not found");
                    Logger.Log($"City not found: {cityName}");
                    return null;
                }

                var first = results[0];
                City city = new City
                {
                    Name = first.GetProperty("name").GetString() ?? cityName,
                    Country = first.GetProperty("country").GetString() ?? "",
                    Latitude = first.GetProperty("latitude").GetDouble(),
                    Longitude = first.GetProperty("longitude").GetDouble()
                };

                Logger.Log($"City found: {city.Name}, {city.Country} ({city.Latitude},{city.Longitude})");
                return city;
            }
            catch (HttpRequestException)
            {
                ToastNotifier.ShowError("No internet connection");
                Logger.Log("Geocoding API request failed: No internet");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Unexpected error in GeocodingService");
                ToastNotifier.ShowError("An error occurred while fetching city data");
            }

            return null;
        }
    }
}
