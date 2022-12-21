using System;

namespace FanControl.Common
{
    public class ConsoleLogger : ILogger
    {
        public void Error(Exception ex)
        {
            Error(ex.ToString());
        }

        public void Error(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow: yyyy-MM-ddTHH:mm:ss} Error: {message}");
        }

        public void Info(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow: yyyy-MM-ddTHH:mm:ss} Info: {message}");
        }

        public void Trace(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow: yyyy-MM-ddTHH:mm:ss} Trace: {message}");
        }

        public void Warning(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow: yyyy-MM-ddTHH:mm:ss} Warning: {message}");
        }
    }
}