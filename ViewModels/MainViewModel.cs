using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherDashboard.Models;
using WeatherDashboard.Services;
using WeatherDashboard.Helpers;

namespace WeatherDashboard.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GeocodingService _geocodingService;
        private readonly WeatherApiService _weatherService;

        public MainViewModel()
        {
            _currentWeather = null;
            _isLoading = false;
            _weeklyForecast = null;
            _cityName = string.Empty;
            _selectedCity = null;
            _geocodingService = new GeocodingService();
            _weatherService = new WeatherApiService();
            ToggleWeeklyForecastCommand = new AsyncCommand(ToggleWeeklyForecastAsync);
            SearchCommand = new AsyncCommand(SearchAsync);
        }

        // ======================
        // Bindable Properties
        // ======================

        private string _cityName = string.Empty;
        public string CityName
        {
            get => _cityName;
            set { _cityName = value; OnPropertyChanged(); }
        }
        private City? _selectedCity;
        public City? SelectedCity
        {
            get => _selectedCity;
            set { _selectedCity = value; OnPropertyChanged(); OnPropertyChanged(nameof(CityDisplay)); }
        }
        private List<DailyForecast>? _weeklyForecast;
        public List<DailyForecast>? WeeklyForecast
        {
            get => _weeklyForecast;
            set
            {
                _weeklyForecast = value;
                OnPropertyChanged();
            }
        }

        public string CityDisplay => SelectedCity != null ? $"{SelectedCity.Name}, {SelectedCity.Country}" : "";

        private CurrentWeather? _currentWeather;
        public CurrentWeather? CurrentWeather
        {
            get => _currentWeather;
            set { _currentWeather = value; OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
private bool _isWeeklyVisible = true;
public bool IsWeeklyVisible
{
    get => _isWeeklyVisible;
    set { _isWeeklyVisible = value; OnPropertyChanged(); }
}

// Command to toggle weekly forecast
public ICommand ToggleWeeklyForecastCommand { get; }

// The method:
private Task ToggleWeeklyForecastAsync()
{
    IsWeeklyVisible = !IsWeeklyVisible;
    return Task.CompletedTask;
}


        // ======================
        // Commands
        // ======================

        public ICommand SearchCommand { get; }

        private async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(CityName))
            {
                ToastNotifier.ShowError("Please enter a city name");
                return;
            }

            try
            {
                IsLoading = true;

                City? city = await _geocodingService.GetCityAsync(CityName);
                if (city == null)
                    return;

                var result = await _weatherService.GetWeatherAsync(city);
                SelectedCity = city;
                CurrentWeather = result.current;
                WeeklyForecast = result.daily;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Error in MainViewModel");
                ToastNotifier.ShowError("Unexpected error occurred");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ======================
        // INotifyPropertyChanged
        // ======================

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // ==========================================
    // Simple async ICommand (INLINE, NO FILE)
    // ==========================================

    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private bool _isExecuting;

        public AsyncCommand(Func<Task> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter) => !_isExecuting;

        public async void Execute(object? parameter)
        {
            if (_isExecuting) return;

            try
            {
                _isExecuting = true;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? CanExecuteChanged;
    }
}
