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
private static bool IsBlockedRegion(string? country)
{
    if (string.IsNullOrWhiteSpace(country))
        return false;

    country = country.Trim().ToLowerInvariant();

    return country == "israel"
        || country == "il";
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
        var response = await _httpClient.GetAsync(
            $"search?name={Uri.EscapeDataString(cityName)}&count=1");

        if (!response.IsSuccessStatusCode)
        {
            Logger.Log($"Geocoding API error: {response.StatusCode}");
            ToastNotifier.ShowError("Failed to fetch city info");
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("results", out var results) ||
            results.GetArrayLength() == 0)
        {
            ToastNotifier.ShowError("City not found");
            Logger.Log($"City not found: {cityName}");
            return null;
        }

        var first = results[0];

        string country =
            first.TryGetProperty("country", out var c)
                ? c.GetString() ?? ""
                : "";

        if (IsBlockedRegion(country))
        {
            ToastNotifier.ShowError(
                "Weather data is not available for this location due to data provider limitations.");

            Logger.Log($"Blocked location searched: {cityName} ({country})");
            return null;
        }

        City city = new City
        {
            Name = first.GetProperty("name").GetString() ?? cityName,
            Country = country,
            Latitude = first.GetProperty("latitude").GetDouble(),
            Longitude = first.GetProperty("longitude").GetDouble()
        };

        Logger.Log($"City accepted: {city.Name}, {city.Country}");
        return city;
    }
    catch (HttpRequestException)
    {
        ToastNotifier.ShowError("No internet connection");
        Logger.Log("Geocoding API request failed");
    }
    catch (Exception ex)
    {
        ToastNotifier.ShowError("Unexpected error");
        Logger.Log(ex, "GeocodingService error");
    }

    return null;
}
    }
}
