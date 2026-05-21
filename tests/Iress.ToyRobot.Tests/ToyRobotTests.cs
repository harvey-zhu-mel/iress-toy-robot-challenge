namespace Iress.ToyRobot.Tests;

using Iress.ToyRobot.Core;
using Xunit;

public class ToyRobotTests
{
    [Fact]
    public void Report_ReturnsNull_WhenRobotHasNotBeenPlaced()
    {
        var robot = new ToyRobot();

        var report = robot.Report();

        Assert.Null(report);
    }

    [Fact]
    public void Place_SetsRobotState_WhenPositionIsValid()
    {
        var robot = new ToyRobot();

        robot.Place(new Position(1, 2), Direction.East);

        Assert.Equal("1,2,EAST", robot.Report());
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    public void Place_IgnoresPlacement_WhenPositionIsInvalid(int x, int y)
    {
        var robot = new ToyRobot();

        robot.Place(new Position(x, y), Direction.North);

        Assert.Null(robot.Report());
    }

    [Fact]
    public void Place_ReplacesCurrentState_WhenSecondPlacementIsValid()
    {
        var robot = new ToyRobot();

        robot.Place(new Position(0, 0), Direction.North);
        robot.Move();
        robot.Place(new Position(3, 3), Direction.West);

        Assert.Equal("3,3,WEST", robot.Report());
    }

    [Fact]
    public void Place_DoesNotClearCurrentState_WhenSecondPlacementIsInvalid()
    {
        var robot = new ToyRobot();

        robot.Place(new Position(1, 1), Direction.North);
        robot.Place(new Position(9, 9), Direction.South);

        Assert.Equal("1,1,NORTH", robot.Report());
    }

    [Theory]
    [InlineData(1, 1, Direction.North, "1,2,NORTH")]
    [InlineData(1, 1, Direction.East, "2,1,EAST")]
    [InlineData(1, 1, Direction.South, "1,0,SOUTH")]
    [InlineData(1, 1, Direction.West, "0,1,WEST")]
    public void Move_UpdatesPosition_WhenMoveStaysOnTable(
        int x,
        int y,
        Direction direction,
        string expectedReport)
    {
        var robot = new ToyRobot();
        robot.Place(new Position(x, y), direction);

        robot.Move();

        Assert.Equal(expectedReport, robot.Report());
    }

    [Theory]
    [InlineData(0, 0, Direction.South, "0,0,SOUTH")]
    [InlineData(0, 0, Direction.West, "0,0,WEST")]
    [InlineData(4, 4, Direction.North, "4,4,NORTH")]
    [InlineData(4, 4, Direction.East, "4,4,EAST")]
    public void Move_IgnoresMove_WhenMoveWouldFallOffTable(
        int x,
        int y,
        Direction direction,
        string expectedReport)
    {
        var robot = new ToyRobot();
        robot.Place(new Position(x, y), direction);

        robot.Move();

        Assert.Equal(expectedReport, robot.Report());
    }

    [Fact]
    public void Move_DoesNothing_WhenRobotHasNotBeenPlaced()
    {
        var robot = new ToyRobot();

        robot.Move();

        Assert.Null(robot.Report());
    }

    [Fact]
    public void TurnLeft_RotatesRobot_WhenRobotHasBeenPlaced()
    {
        var robot = new ToyRobot();
        robot.Place(new Position(0, 0), Direction.North);

        robot.TurnLeft();

        Assert.Equal("0,0,WEST", robot.Report());
    }

    [Fact]
    public void TurnRight_RotatesRobot_WhenRobotHasBeenPlaced()
    {
        var robot = new ToyRobot();
        robot.Place(new Position(0, 0), Direction.North);

        robot.TurnRight();

        Assert.Equal("0,0,EAST", robot.Report());
    }

    [Fact]
    public void TurnCommands_DoNothing_WhenRobotHasNotBeenPlaced()
    {
        var robot = new ToyRobot();

        robot.TurnLeft();
        robot.TurnRight();

        Assert.Null(robot.Report());
    }

}