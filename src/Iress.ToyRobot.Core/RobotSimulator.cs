namespace Iress.ToyRobot.Core;

public sealed class RobotSimulator
{
    private readonly CommandParser _parser;
    private readonly ToyRobot _robot;

    public RobotSimulator(CommandParser? parser = null, ToyRobot? robot = null)
    {
        _parser = parser ?? new CommandParser();
        _robot = robot ?? new ToyRobot();
    }

    public IReadOnlyList<string> Process(IEnumerable<string> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        var reports = new List<string>();

        foreach (var line in lines)
        {
            var command = _parser.Parse(line);

            if (command is null)
            {
                continue;
            }

            switch (command)
            {
                case PlaceCommand place:
                    _robot.Place(place.Position, place.Direction);
                    break;

                case MoveCommand:
                    _robot.Move();
                    break;

                case LeftCommand:
                    _robot.TurnLeft();
                    break;

                case RightCommand:
                    _robot.TurnRight();
                    break;

                case ReportCommand:
                    var report = _robot.Report();

                    if (report is not null)
                    {
                        reports.Add(report);
                    }

                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported command type: {command.GetType().Name}");
            }
        }

        return reports;
    }
}