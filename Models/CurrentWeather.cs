namespace WeatherDashboard.Models
{
    public class CurrentWeather
    {
        public double Temperature { get; set; }           // °C
        public double Humidity { get; set; }              // %
        public double WindSpeed { get; set; }             // km/h
        public int WindDirection { get; set; }            // degrees
        public string Condition { get; set; } = string.Empty; // e.g., Clear, Cloudy
        public string WeatherCode { get; set; } = string.Empty; // For icon mapping
    }
}
