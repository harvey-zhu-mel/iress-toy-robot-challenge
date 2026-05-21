namespace Iress.ToyRobot.Core;

public abstract record RobotCommand
{
}

public sealed record PlaceCommand(Position Position, Direction Direction) : RobotCommand;

public sealed record MoveCommand : RobotCommand
{
}

public sealed record LeftCommand : RobotCommand
{
}

public sealed record RightCommand : RobotCommand
{
}

public sealed record ReportCommand : RobotCommand
{
}