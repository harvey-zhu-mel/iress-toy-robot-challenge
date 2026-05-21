namespace Iress.ToyRobot.Core;

public readonly record struct RobotState(Position Position, Direction Direction)
{
   public string ToReportString()
    {
        return $"{Position.X},{Position.Y},{Direction.ToReportValue()}";
    }
}