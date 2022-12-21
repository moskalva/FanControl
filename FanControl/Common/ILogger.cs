using FanControl.Common;

namespace FanControl
{
    public interface ILogger
    {
        void Error(string message);
        void Error(Exception ex);
        void Trace(string message);
        void Warning(string message);
        void Info(string message);
    }

    public static class LoggerProvider
    {
        private static ILogger? logger;
        public static ILogger Init()
        {
            logger = new ConsoleLogger();
            return logger;
        }

        public static ILogger GetInstance()
        {
            if (logger == null)
            {
                throw new InvalidOperationException("Logger not initialized");
            }
            return logger;
        }
    }
}