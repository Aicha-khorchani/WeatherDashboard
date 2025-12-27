using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using WeatherDashboard.Models;

namespace WeatherDashboard.Helpers
{
    public static class PlotFactory
    {
        public static PlotModel CreateTemperaturePlot(DailyForecast day)
        {
            var model = new PlotModel { Title = "Temperature" };
            var series = new LineSeries();

            int hour = 0;
            foreach (var p in day.HourlyPoints)
            {
                series.Points.Add(new DataPoint(hour++, p.Temperature));
            }

            model.Series.Add(series);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "°C" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Hour" });

            return model;
        }

        public static PlotModel CreateWindPlot(DailyForecast day)
        {
            var model = new PlotModel { Title = "Wind Speed" };
            var series = new LineSeries();

            int hour = 0;
            foreach (var p in day.HourlyPoints)
            {
                series.Points.Add(new DataPoint(hour++, p.WindSpeed));
            }

            model.Series.Add(series);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "km/h" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Hour" });

            return model;
        }
    }
}
