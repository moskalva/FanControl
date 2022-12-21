
using System.Device.Gpio;
using Iot.Device.CpuTemperature;

public enum Operation
{
    Unknown,
    On,
    Off,
}


static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Blinking LED. Enter pin number and 'On'/'Off' to manage GPIO pin. Press Ctrl+C to end.");

        var pin = ReadArgs(args);

        Console.WriteLine($"Selected pin '{pin}'");

        using var controller = new GpioController();
        controller.OpenPin(pin, PinMode.Output);
        while (true)
        {
            var command = ReadCommand(Console.ReadLine());

            if (command == Operation.Unknown)
            {
                continue;
            }

            var pinValue = command == Operation.On ? PinValue.High
                      : command == Operation.Off ? PinValue.Low
                      : throw new NotSupportedException($"Unknown operation '{command}'");
            var c = new CpuTemperature();
            controller.Write(pin, pinValue);
        }
    }

    static int ReadArgs(string[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Invalid arguments, pin expected.");
        }

        if (!int.TryParse(args[0], out var pin))
        {
            throw new ArgumentException("Cannot parse pin.");
        }

        if (pin < 0 || pin > 27)
        {
            throw new ArgumentException("Unknown pin");
        }
        return pin;
    }

    static Operation ReadCommand(string? args)
    {
        return args?.ToLower() switch
        {
            "on" => Operation.On,
            "off" => Operation.Off,
            _ => Operation.Unknown,
        };
    }
}