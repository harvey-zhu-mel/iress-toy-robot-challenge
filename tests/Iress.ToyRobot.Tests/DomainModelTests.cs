namespace Iress.ToyRobot.Tests;

using Iress.ToyRobot.Core;
using Xunit;

public class DomainModelTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(4, 4)]
    [InlineData(2, 3)]
    public void StandardTable_Contains_ReturnsTrue_WhenPositionIsInsideBounds(int x, int y)
    {
        var table = TableTop.Standard;

        var result = table.Contains(new Position(x, y));

        Assert.True(result);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(5, 0)]
    [InlineData(0, 5)]
    [InlineData(5, 5)]
    public void StandardTable_Contains_ReturnsFalse_WhenPositionIsOutsideBounds(int x, int y)
    {
        var table = TableTop.Standard;

        var result = table.Contains(new Position(x, y));

        Assert.False(result);
    }

    [Theory]
    [InlineData(Direction.North, Direction.West)]
    [InlineData(Direction.West, Direction.South)]
    [InlineData(Direction.South, Direction.East)]
    [InlineData(Direction.East, Direction.North)]
    public void TurnLeft_ReturnsExpectedDirection(Direction current, Direction expected)
    {
        Assert.Equal(expected, current.TurnLeft());
    }

    [Theory]
    [InlineData(Direction.North, Direction.East)]
    [InlineData(Direction.East, Direction.South)]
    [InlineData(Direction.South, Direction.West)]
    [InlineData(Direction.West, Direction.North)]
    public void TurnRight_ReturnsExpectedDirection(Direction current, Direction expected)
    {
        Assert.Equal(expected, current.TurnRight());
    }

    [Theory]
    [InlineData(Direction.North, "NORTH")]
    [InlineData(Direction.East, "EAST")]
    [InlineData(Direction.South, "SOUTH")]
    [InlineData(Direction.West, "WEST")]
    public void ToReportValue_ReturnsUppercaseDirection(Direction direction, string expected)
    {
        Assert.Equal(expected, direction.ToReportValue());
    }
}