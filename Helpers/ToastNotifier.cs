using System.Windows;
using System.Windows.Threading;

namespace WeatherDashboard.Helpers
{
    public static class ToastNotifier
    {
        /// <summary>
        /// Shows a temporary toast message
        /// </summary>
        /// <param name="message">Text to display</param>
        /// <param name="title">Title of toast</param>
        /// <param name="durationSeconds">How long to show</param>
        public static void Show(string message, string title = "Notification", int durationSeconds = 3)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Create a simple Window for toast
                Window toast = new Window
                {
                    Width = 300,
                    Height = 100,
                    Topmost = true,
                    WindowStyle = WindowStyle.None,
                    AllowsTransparency = true,
                    Background = System.Windows.Media.Brushes.LightGray,
                    Content = new System.Windows.Controls.TextBlock
                    {
                        Text = $"{title}\n{message}",
                        TextWrapping = System.Windows.TextWrapping.Wrap,
                        Margin = new Thickness(10),
                        FontSize = 14
                    },
                    ShowInTaskbar = false,
                    Opacity = 0.9
                };

                // Position bottom-right
                var workingArea = System.Windows.SystemParameters.WorkArea;
                toast.Left = workingArea.Right - toast.Width - 10;
                toast.Top = workingArea.Bottom - toast.Height - 10;

                toast.Show();

                // Close after duration
                DispatcherTimer timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(durationSeconds)
                };
                timer.Tick += (s, e) =>
                {
                    toast.Close();
                    timer.Stop();
                };
                timer.Start();
            });
        }

        /// <summary>
        /// Shows an error toast
        /// </summary>
        public static void ShowError(string message, int durationSeconds = 3)
        {
            Show(message, "Error", durationSeconds);
        }

        /// <summary>
        /// Shows a success toast
        /// </summary>
        public static void ShowSuccess(string message, int durationSeconds = 3)
        {
            Show(message, "Success", durationSeconds);
        }
    }
}
