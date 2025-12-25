using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherDashboard.Models;
using WeatherDashboard.ViewModels;
namespace WeatherDashboard.Views
{
public partial class WeeklyForecastView : UserControl
{
public WeeklyForecastView(List<DailyForecast> forecasts)
    {
        InitializeComponent();
        this.DataContext = new WeeklyForecastViewModel(forecasts);
    }
}
}