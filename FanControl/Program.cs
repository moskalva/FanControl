using System.Threading.Tasks;
using FanControl;
using FanControl.App;
using FanControl.Common;
using FanControl.PubSub;

public enum Mode
{
    Auto,
    Manual,
}
static class Program
{
    static readonly string Help = @"Example:
    & FanControl Manual 15
    & FanControl Auto 17
    & FanControl Auto 17 50 47
    ";
    static TimeSpan SleepTime = TimeSpan.FromSeconds(10);

    static ushort StartTemp = 53;
    static ushort StopTemp = 47;

    public static void Main(string[] args)
    {
        if (args == null || args.Length < 2 ||
          !Enum.TryParse<Mode>(args[0], out var mode) ||
          !ushort.TryParse(args[1], out var pin) ||
          (args.Length > 2 && mode == Mode.Auto && (
                !ushort.TryParse(args[2], out StartTemp) ||
                !ushort.TryParse(args[3], out StopTemp))))
        {
            throw new ArgumentException(@"Invalid parameters. " + Help);
        }
        var cancellation = new CancellationTokenSource();
        var t = Setup(mode, pin, cancellation.Token);

        if (mode == Mode.Manual)
        {
            Console.WriteLine("Press 'Enter' to exit.");
            Console.ReadLine();
            Console.WriteLine("Exiting.");
            cancellation.Cancel();
        }
        else
        {
            Console.WriteLine("Running as a service.");
        }
        t.Wait();
    }

    public static Task Setup(
        Mode mode,
        ushort pin,
        CancellationToken token)
    {
        var logger = LoggerProvider.Init();

        var fanSettings = new FanControllerSettings(pin, StartTemp, StopTemp);

        var publisher = new PubSubManager<TemperatureEvent>();
        var fancontroller = new FanController(fanSettings);
        var tempController = new TemperatureControllerAdapter(
            new TemperatureControllerAdapterSettings(SleepTime),
            new SkipDuplicatesPublisher<TemperatureEvent>(publisher));

        publisher.Subscribe(fancontroller);
        publisher.Subscribe(new LoggerSubscriber(logger));

        logger.Info($"Starting fanc controller with mode '{mode}' on pin '{pin}'");
        return Task.Run(() =>
        {
            tempController.Start();
            logger.Info("Started fan controller.");
            token.WaitHandle.WaitOne();
            logger.Trace("Exit runner thread.");
        });
    }
}
