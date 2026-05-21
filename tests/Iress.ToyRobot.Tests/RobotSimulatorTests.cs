namespace Iress.ToyRobot.Tests;

using Iress.ToyRobot.Core;
using Xunit;

public class RobotSimulatorTests
{
    [Fact]
    public void Process_ReturnsExpectedOutput_ForChallengeExampleA()
    {
        var output = Process(
            "PLACE 0,0,NORTH",
            "MOVE",
            "REPORT");

        Assert.Equal(new[] { "0,1,NORTH" }, output);
    }

    [Fact]
    public void Process_ReturnsExpectedOutput_ForChallengeExampleB()
    {
        var output = Process(
            "PLACE 0,0,NORTH",
            "LEFT",
            "REPORT");

        Assert.Equal(new[] { "0,0,WEST" }, output);
    }

    [Fact]
    public void Process_ReturnsExpectedOutput_ForChallengeExampleC()
    {
        var output = Process(
            "PLACE 1,2,EAST",
            "MOVE",
            "MOVE",
            "LEFT",
            "MOVE",
            "REPORT");

        Assert.Equal(new[] { "3,3,NORTH" }, output);
    }

    [Fact]
    public void Process_IgnoresCommandsBeforeFirstValidPlace()
    {
        var output = Process(
            "MOVE",
            "LEFT",
            "RIGHT",
            "REPORT",
            "PLACE 1,1,EAST",
            "REPORT");

        Assert.Equal(new[] { "1,1,EAST" }, output);
    }

    [Fact]
    public void Process_IgnoresInvalidCommands()
    {
        var output = Process(
            "HELLO",
            "PLACE 0,0,NORTH",
            "BAD COMMAND",
            "MOVE",
            "REPORT");

        Assert.Equal(new[] { "0,1,NORTH" }, output);
    }

    [Fact]
    public void Process_ProducesNoOutput_WhenReportOccursBeforeValidPlace()
    {
        var output = Process(
            "REPORT",
            "PLACE 9,9,NORTH",
            "REPORT");

        Assert.Empty(output);
    }

    [Fact]
    public void Process_ReturnsMultipleReports_WhenMultipleReportCommandsAreValid()
    {
        var output = Process(
            "PLACE 0,0,NORTH",
            "REPORT",
            "MOVE",
            "REPORT",
            "RIGHT",
            "REPORT");

        Assert.Equal(
            new[]
            {
                "0,0,NORTH",
                "0,1,NORTH",
                "0,1,EAST"
            },
            output);
    }

    private static IReadOnlyList<string> Process(params string[] lines)
    {
        var simulator = new RobotSimulator();

        return simulator.Process(lines);
    }
}