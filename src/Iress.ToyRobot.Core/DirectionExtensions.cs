namespace Iress.ToyRobot.Core;

public static class DirectionExtensions
{
    public static Direction TurnLeft(this Direction direction)
    {
        return (Direction)(((int)direction + 3) % 4);
    }

    public static Direction TurnRight(this Direction direction)
    {
        return (Direction)(((int)direction + 1) % 4);
    }

    public static string ToReportValue(this Direction direction)
    {
        return direction switch
        {
            Direction.North => "NORTH",
            Direction.East => "EAST",
            Direction.South => "SOUTH",
            Direction.West => "WEST",
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported direction."),
        };
    }

    public static bool TryParseDirection(string? input, out Direction direction)
    {
        direction = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        switch (input.Trim())
        {
            case "NORTH":
                direction = Direction.North;
                return true;
            case "EAST":
                direction = Direction.East;
                return true;
            case "SOUTH":
                direction = Direction.South;
                return true;
            case "WEST":
                direction = Direction.West;
                return true;
            default:
                return false;
        }
    }

}