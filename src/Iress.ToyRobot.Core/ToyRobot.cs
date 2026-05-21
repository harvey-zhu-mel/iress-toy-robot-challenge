namespace Iress.ToyRobot.Core;

public sealed class ToyRobot
{
    private readonly TableTop _tableTop;
    private RobotState? _state;

    public ToyRobot(TableTop? tableTop = null)
    {
        _tableTop = tableTop ?? TableTop.Standard;
    }

    public RobotState? State => _state;

    public void Place(Position position, Direction direction)
    {
        if (!_tableTop.IsValidPosition(position))
        {
            return;
        }
        _state = new RobotState(position, direction);
    }

    public void Move()
    {
        if(! _state.HasValue)
        {
            return;
        }

        var currentState = _state.Value;
        var nextPosition = currentState.Direction switch
        {
            Direction.North => new Position(currentState.Position.X, currentState.Position.Y + 1),
            Direction.East => new Position(currentState.Position.X + 1, currentState.Position.Y),
            Direction.South => new Position(currentState.Position.X, currentState.Position.Y - 1),
            Direction.West => new Position(currentState.Position.X - 1, currentState.Position.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(currentState.Position),  currentState.Direction, "Unsupported robot direction.")
        };

        if (!_tableTop.IsValidPosition(nextPosition))
        {
            return;
        }

        _state = currentState with { Position = nextPosition };
    }

    public void TurnLeft()
    {
        if (!_state.HasValue)
        {
            return;
        }

        var currentState = _state.Value;
        var newDirection = currentState.Direction.TurnLeft();

        _state = currentState with { Direction = newDirection };
    }

    public void TurnRight()
    {
        if (!_state.HasValue)
        {
            return;
        }

        var currentState = _state.Value;
        var newDirection = currentState.Direction.TurnRight();

        _state = currentState with { Direction = newDirection };
    }

    public string? Report()
    {
        if (!_state.HasValue)
        {
            return null;
        }

        return _state.Value.ToReportString();
    }
   
}