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

namespace WeatherDashboard.Views
{
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
         var vm = new MainViewModel();
            DataContext = vm;

            // Subscribe to property changed for WeeklyForecast updates
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.WeeklyForecast))
                {
                    // Set DataContext of WeeklyForecastControl when WeeklyForecast updates
                    WeeklyForecastControl.DataContext = new WeeklyForecastViewModel(vm.WeeklyForecast);
                }
            };
    }
}
}