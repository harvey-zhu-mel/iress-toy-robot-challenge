using Iress.ToyRobot.Core;

IEnumerable<string> inputLines;

if (args.Length == 1 && IsHelpArgument(args[0]))
{
    WriteUsage(Console.Out);
    return 0;
}

if (args.Length > 1)
{
    Console.Error.WriteLine("Invalid arguments.");
    WriteUsage(Console.Error);
    return 1;
}

if (args.Length == 1)
{
    var path = args[0];

    if (!File.Exists(path))
    {
        if (IsRobotCommandArgument(path))
        {
            Console.Error.WriteLine("Robot commands are read from standard input or an input file, not from command-line arguments.");
            Console.Error.WriteLine("Run the application with no arguments, then enter commands one per line.");
            Console.Error.WriteLine();
            WriteExamples(Console.Error);
            return 1;
        }

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

static bool IsHelpArgument(string argument)
{
    return string.Equals(argument, "--help", StringComparison.OrdinalIgnoreCase)
        || string.Equals(argument, "-h", StringComparison.OrdinalIgnoreCase);
}

static bool IsRobotCommandArgument(string argument)
{
    var value = argument.TrimStart('-').Trim();
    var firstSeparatorIndex = value.IndexOfAny([' ', '\t']);
    var commandName = firstSeparatorIndex < 0
        ? value
        : value[..firstSeparatorIndex];

    return string.Equals(commandName, "PLACE", StringComparison.OrdinalIgnoreCase)
        || string.Equals(commandName, "MOVE", StringComparison.OrdinalIgnoreCase)
        || string.Equals(commandName, "LEFT", StringComparison.OrdinalIgnoreCase)
        || string.Equals(commandName, "RIGHT", StringComparison.OrdinalIgnoreCase)
        || string.Equals(commandName, "REPORT", StringComparison.OrdinalIgnoreCase);
}

static void WriteUsage(TextWriter writer)
{
    writer.WriteLine("Usage: Iress.ToyRobot.Console [input-file]");
    writer.WriteLine();
    writer.WriteLine("Reads toy robot commands from an optional input file, or from standard input when no file is supplied.");
    writer.WriteLine("Robot commands are entered one per line. They are not command-line arguments.");
    writer.WriteLine();
    writer.WriteLine("Input modes:");
    writer.WriteLine("  Iress.ToyRobot.Console                 Read commands from standard input.");
    writer.WriteLine("  Iress.ToyRobot.Console commands.txt    Read commands from a file.");
    writer.WriteLine();
    writer.WriteLine("Commands:");
    WriteCommandExamples(writer);
    writer.WriteLine();
    writer.WriteLine("Example standard input session:");
    WriteExamples(writer);
    writer.WriteLine();
    writer.WriteLine("Options:");
    writer.WriteLine("  -h, --help    Show this help message.");
}

static void WriteCommandExamples(TextWriter writer)
{
    writer.WriteLine("  PLACE 1,2,NORTH");
    writer.WriteLine("  MOVE");
    writer.WriteLine("  LEFT");
    writer.WriteLine("  RIGHT");
    writer.WriteLine("  REPORT");
}

static void WriteExamples(TextWriter writer)
{
    writer.WriteLine("  $ dotnet run --project ./src/Iress.ToyRobot.Console");
    writer.WriteLine("  PLACE 1,2,NORTH");
    writer.WriteLine("  MOVE");
    writer.WriteLine("  REPORT");
    writer.WriteLine();
    writer.WriteLine("End standard input with Ctrl+D on macOS/Linux, or Ctrl+Z then Enter on Windows.");
}
