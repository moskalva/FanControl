using System.Timers;
using FanControl.PubSub;
using Iot.Device.CpuTemperature;

namespace FanControl.App
{
    public record struct TemperatureEvent(ushort temperature);

    public record class TemperatureControllerAdapterSettings(
        TimeSpan measurementDelay)
    {
        public static readonly TemperatureControllerAdapterSettings Default = new TemperatureControllerAdapterSettings(TimeSpan.FromSeconds(10));
    }

    public class TemperatureControllerAdapter:IDisposable
    {
        private static readonly ILogger Log = LoggerProvider.GetInstance(); 
        
        private readonly System.Timers.Timer timer;
        private readonly IPublisher<TemperatureEvent> publisher;
        private readonly CpuTemperature controller = new CpuTemperature();

        public TemperatureControllerAdapter(
            TemperatureControllerAdapterSettings settings,
            IPublisher<TemperatureEvent> publisher)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

            this.timer = new System.Timers.Timer(settings.measurementDelay.TotalMilliseconds);
            this.timer.AutoReset = true;
            this.timer.Elapsed += OnTimer;
        }

        public void Start()
        {
            Log.Trace("Timer started");
            this.timer.Start();
        }
        public void Stop()
        {
            Log.Trace("Timer stopped");
            this.timer.Stop();
        }

        private void OnTimer(object? source, ElapsedEventArgs args)
        {
            Log.Trace("Timer triggered");
            var temperature = (ushort)controller.Temperature.DegreesCelsius;
            this.publisher.Publish(new TemperatureEvent(temperature));
        }

        public void Dispose(){
            this.Stop();
            this.controller.Dispose();
        }
    }
}