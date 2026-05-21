using Iress.ToyRobot.Core;

IEnumerable<string> inputLines;

if (args.Length > 1)
{
    Console.Error.WriteLine("Usage: Iress.ToyRobot.Console [input-file]");
    return 1;
}

if (args.Length == 1)
{
    var path = args[0];

    if (!File.Exists(path))
    {
        Console.Error.WriteLine($"Input file not found: {path}");
        return 1;
    }

    inputLines = File.ReadLines(path);
}
else
{
    inputLines = ReadStandardInput();
}

var simulator = new RobotSimulator();

foreach (var output in simulator.Process(inputLines))
{
    Console.WriteLine(output);
}

return 0;

static IEnumerable<string> ReadStandardInput()
{
    string? line;

    while ((line = Console.ReadLine()) is not null)
    {
        yield return line;
    }
}