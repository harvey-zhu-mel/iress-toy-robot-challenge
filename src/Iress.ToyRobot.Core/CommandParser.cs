namespace Iress.ToyRobot.Core;

using System.Globalization;

public sealed class CommandParser
{
    private static readonly char[] CommandSeparators = { ' ', '\t' }; // Define separators as 'white space' or 'tab' character for splitting command name and arguments.

    public RobotCommand? Parse(string? line)
    {
        if (!TrySplitCommandLine(line, out var commandName, out var arguments))
        {
            return null;
        }

        if (IsCommand(commandName, "PLACE"))
        {
            return ParsePlace(arguments);
        }

        if (!string.IsNullOrWhiteSpace(arguments))
        {
            return null;
        }

        return ParseSimpleCommand(commandName);
    }

    private static RobotCommand? ParseSimpleCommand(string commandName)
    {
        if (IsCommand(commandName, "MOVE"))
        {
            return new MoveCommand();
        }

        if (IsCommand(commandName, "LEFT"))
        {
            return new LeftCommand();
        }

        if (IsCommand(commandName, "RIGHT"))
        {
            return new RightCommand();
        }

        if (IsCommand(commandName, "REPORT"))
        {
            return new ReportCommand();
        }

        return null;
    }

    private static RobotCommand? ParsePlace(string arguments)
    {
        if (!TryParsePlaceArguments(arguments, out var x, out var y, out var direction))
        {
            return null;
        }

        return new PlaceCommand(new Position(x, y), direction);
    }

    private static bool TrySplitCommandLine(
        string? line,
        out string commandName,
        out string arguments)
    {
        commandName = string.Empty;
        arguments = string.Empty;

        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        var trimmedLine = line.Trim();
        var firstSeparatorIndex = trimmedLine.IndexOfAny(CommandSeparators);

        if (firstSeparatorIndex < 0)
        {
            commandName = trimmedLine;
            return true;
        }

        commandName = trimmedLine[..firstSeparatorIndex];
        arguments = trimmedLine[(firstSeparatorIndex + 1)..].Trim();

        return true;
    }

    private static bool TryParsePlaceArguments(
        string arguments,
        out int x,
        out int y,
        out Direction direction)
    {
        x = default;
        y = default;
        direction = default;

        if (string.IsNullOrWhiteSpace(arguments))
        {
            return false;
        }

        var parts = arguments.Split(',', StringSplitOptions.TrimEntries);

        if (parts.Length != 3)
        {
            return false;
        }

        if (!TryParseCoordinate(parts[0], out x))
        {
            return false;
        }

        if (!TryParseCoordinate(parts[1], out y))
        {
            return false;
        }

        if (!DirectionExtensions.TryParseDirection(parts[2], out direction))
        {
            return false;
        }

        return true;
    }

    private static bool TryParseCoordinate(string value, out int coordinate)
    {
        return int.TryParse(
            value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out coordinate);
    }

    private static bool IsCommand(string actual, string expected)
    {
        return string.Equals(
            actual,
            expected,
            StringComparison.OrdinalIgnoreCase);
    }
}