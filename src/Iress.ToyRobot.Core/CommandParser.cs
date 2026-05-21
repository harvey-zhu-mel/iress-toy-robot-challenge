namespace Iress.ToyRobot.Core;

using System.Globalization;

public sealed class CommandParser
{
    public RobotCommand? Parse(string? line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;
        }

        var trimmed = line.Trim();
        var separatorIndex = trimmed.IndexOfAny(new[] { ' ', '\t' });

        var commandName = separatorIndex < 0
            ? trimmed
            : trimmed[..separatorIndex];

        var arguments = separatorIndex < 0
            ? string.Empty
            : trimmed[(separatorIndex + 1)..].Trim();

        return commandName.ToUpperInvariant() switch
        {
            "PLACE" => ParsePlace(arguments),
            "MOVE" when string.IsNullOrWhiteSpace(arguments) => new MoveCommand(),
            "LEFT" when string.IsNullOrWhiteSpace(arguments) => new LeftCommand(),
            "RIGHT" when string.IsNullOrWhiteSpace(arguments) => new RightCommand(),
            "REPORT" when string.IsNullOrWhiteSpace(arguments) => new ReportCommand(),
            _ => null
        };
    }

    private static RobotCommand? ParsePlace(string arguments)
    {
        if (string.IsNullOrWhiteSpace(arguments))
        {
            return null;
        }

        var parts = arguments.Split(',', StringSplitOptions.TrimEntries);

        if (parts.Length != 3)
        {
            return null;
        }

        if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var x))
        {
            return null;
        }

        if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var y))
        {
            return null;
        }

        if (!DirectionExtensions.TryParseDirection(parts[2], out var direction))
        {
            return null;
        }

        return new PlaceCommand(new Position(x, y), direction);
    }
}