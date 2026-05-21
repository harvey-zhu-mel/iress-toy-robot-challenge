namespace Iress.ToyRobot.Tests;

using Iress.ToyRobot.Core;
using Xunit;

public class CommandParserTests
{
    private readonly CommandParser _parser = new();

    [Fact]
    public void Parse_ReturnsPlaceCommand_WhenPlaceCommandIsValid()
    {
        var command = _parser.Parse("PLACE 1,2,EAST");

        var place = Assert.IsType<PlaceCommand>(command);
        Assert.Equal(new Position(1, 2), place.Position);
        Assert.Equal(Direction.East, place.Direction);
    }

    [Theory]
    [InlineData("PLACE 1,2,EAST")]
    [InlineData(" place 1,2,east ")]
    [InlineData("PLACE   1,2,EAST")]
    [InlineData("PLACE 1, 2, EAST")]
    public void Parse_HandlesPlaceCommandWhitespaceAndCase(string input)
    {
        var command = _parser.Parse(input);

        var place = Assert.IsType<PlaceCommand>(command);
        Assert.Equal(new Position(1, 2), place.Position);
        Assert.Equal(Direction.East, place.Direction);
    }

    [Theory]
    [InlineData("MOVE", typeof(MoveCommand))]
    [InlineData("LEFT", typeof(LeftCommand))]
    [InlineData("RIGHT", typeof(RightCommand))]
    [InlineData("REPORT", typeof(ReportCommand))]
    [InlineData("move", typeof(MoveCommand))]
    [InlineData(" left ", typeof(LeftCommand))]
    public void Parse_ReturnsSimpleCommand_WhenInputIsValid(string input, Type expectedType)
    {
        var command = _parser.Parse(input);

        Assert.NotNull(command);
        Assert.IsType(expectedType, command);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("HELLO")]
    [InlineData("PLACE")]
    [InlineData("PLACE 1,2")]
    [InlineData("PLACE 1,2,UP")]
    [InlineData("PLACE A,2,NORTH")]
    [InlineData("PLACE 1,B,NORTH")]
    [InlineData("MOVE 1")]
    [InlineData("LEFT 1")]
    [InlineData("RIGHT 1")]
    [InlineData("REPORT 1")]
    public void Parse_ReturnsNull_WhenInputIsInvalid(string? input)
    {
        var command = _parser.Parse(input);

        Assert.Null(command);
    }
}