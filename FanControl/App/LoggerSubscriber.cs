using FanControl.PubSub;

namespace FanControl.App
{
    public class LoggerSubscriber : ISubscriber<TemperatureEvent>
    {
        private readonly ILogger logger;

        public LoggerSubscriber(ILogger logger){
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnEvent(TemperatureEvent e){
            logger.Info($"Temperature: '{e.temperature}'°C");
        }
    }
}