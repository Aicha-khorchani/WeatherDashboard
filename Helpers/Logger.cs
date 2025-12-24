using System;
using System.IO;

namespace WeatherDashboard.Helpers
{
    public static class Logger
    {
        private static readonly string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string logFile = Path.Combine(logFolder, "log.txt");

        static Logger()
        {
            // Ensure folder exists
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
        }

        /// <summary>
        /// Logs a message with timestamp
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Log(string message)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                
                // Write to console (optional)
                Console.WriteLine(logMessage);

                // Append to log file
                File.AppendAllText(logFile, logMessage + Environment.NewLine);
            }
            catch
            {
                // Fail silently if logging fails
            }
        }

        /// <summary>
        /// Logs an exception with optional message
        /// </summary>
        public static void Log(Exception ex, string? message = null)
        {
            string fullMessage = message == null ? ex.ToString() : $"{message} - {ex}";
            Log(fullMessage);
        }
    }
}
