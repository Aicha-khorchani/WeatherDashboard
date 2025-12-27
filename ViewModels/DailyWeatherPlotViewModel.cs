using System.ComponentModel;
using System.Runtime.CompilerServices;
using OxyPlot;
using WeatherDashboard.Models;
using WeatherDashboard.Helpers;

namespace WeatherDashboard.ViewModels
{
    public class DailyWeatherPlotViewModel : INotifyPropertyChanged
    {
        private DailyForecast? _day;

        public DailyForecast? Day
        {
            get => _day;
            set
            {
                _day = value;
                UpdatePlots();
                OnPropertyChanged();
            }
        }

        private PlotModel _temperaturePlot = new();
        public PlotModel TemperaturePlot
        {
            get => _temperaturePlot;
            private set
            {
                _temperaturePlot = value;
                OnPropertyChanged();
            }
        }

        private PlotModel _windPlot = new();
        public PlotModel WindPlot
        {
            get => _windPlot;
            private set
            {
                _windPlot = value;
                OnPropertyChanged();
            }
        }

        private void UpdatePlots()
        {
            if (Day == null)
                return;

            TemperaturePlot = PlotFactory.CreateTemperaturePlot(Day);
            WindPlot = PlotFactory.CreateWindPlot(Day);
        }

        // ✅ THIS WAS MISSING
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
