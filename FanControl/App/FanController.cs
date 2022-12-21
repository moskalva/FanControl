using FanControl.PubSub;
using System.Device.Gpio;

namespace FanControl.App
{
    public record class FanControllerSettings(
        ushort Pin,
        ushort StartTemp,
        ushort StopTemp
    );

    public class FanController : ISubscriber<TemperatureEvent>, IDisposable
    {
        private static readonly ILogger Log = LoggerProvider.GetInstance();
        private readonly GpioController gpioController = new GpioController();
        private readonly FanControllerSettings settings;
        private bool isOn = false;

        public FanController(FanControllerSettings settings)
        {
            gpioController.OpenPin(settings.Pin, PinMode.Output);
            this.settings = settings;
        }

        public void OnEvent(TemperatureEvent e)
        {
            if (isOn && e.temperature <= this.settings.StopTemp){
                Log.Info("Turning fan off");
                this.gpioController.Write(settings.Pin, PinValue.Low);
                this.isOn = false;
            }
            if (!isOn && e.temperature >= this.settings.StartTemp){
                Log.Info("Turning fan on");
                this.gpioController.Write(settings.Pin, PinValue.High);
                this.isOn = true;
            }
        }
        

        public void Dispose(){
            this.gpioController.Dispose();
        }
    }
}