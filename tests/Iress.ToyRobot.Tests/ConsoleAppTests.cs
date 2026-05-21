namespace Iress.ToyRobot.Tests;

using System.Diagnostics;

public class ConsoleAppTests
{
    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    public void Run_PrintsUsageAndReturnsSuccess_WhenHelpArgumentIsProvided(string argument)
    {
        var result = RunConsole(argument);

        Assert.Equal(0, result.ExitCode);
        Assert.Contains("Usage: Iress.ToyRobot.Console [input-file]", result.StandardOutput);
        Assert.Contains("Robot commands are entered one per line.", result.StandardOutput);
        Assert.Contains("PLACE 1,2,NORTH", result.StandardOutput);
        Assert.Empty(result.StandardError);
    }

    [Fact]
    public void Run_PrintsErrorAndReturnsFailure_WhenMultipleArgumentsAreProvided()
    {
        var result = RunConsole("one", "two");

        Assert.Equal(1, result.ExitCode);
        Assert.Contains("Invalid arguments.", result.StandardError);
        Assert.Contains("Usage: Iress.ToyRobot.Console [input-file]", result.StandardError);
        Assert.Empty(result.StandardOutput);
    }

    [Theory]
    [InlineData("MOVE")]
    [InlineData("--Place 1, 2, North")]
    public void Run_PrintsGuidanceAndReturnsFailure_WhenRobotCommandIsProvidedAsArgument(string argument)
    {
        var result = RunConsole(argument);

        Assert.Equal(1, result.ExitCode);
        Assert.Contains("Robot commands are read from standard input or an input file", result.StandardError);
        Assert.Contains("dotnet run --project ./src/Iress.ToyRobot.Console", result.StandardError);
        Assert.Empty(result.StandardOutput);
    }

    [Fact]
    public void Run_PrintsErrorAndReturnsFailure_WhenInputFileDoesNotExist()
    {
        var result = RunConsole("missing-file.txt");

        Assert.Equal(1, result.ExitCode);
        Assert.Contains("Input file not found: missing-file.txt", result.StandardError);
        Assert.Empty(result.StandardOutput);
    }

    [Fact]
    public void Run_ProcessesCommandsFromInputFile()
    {
        var inputFile = Path.Combine(FindSolutionRoot(), "testdata", "example-c.txt");

        var result = RunConsole(inputFile);

        Assert.Equal(0, result.ExitCode);
        Assert.Equal($"3,3,NORTH{Environment.NewLine}", result.StandardOutput);
        Assert.Empty(result.StandardError);
    }

    [Fact]
    public void Run_ProcessesCommandsFromStandardInput()
    {
        var result = RunConsoleWithStandardInput(string.Join(
            Environment.NewLine,
            "PLACE 0,0,NORTH",
            "MOVE",
            "REPORT"));

        Assert.Equal(0, result.ExitCode);
        Assert.Equal($"0,1,NORTH{Environment.NewLine}", result.StandardOutput);
        Assert.Empty(result.StandardError);
    }

    private static ConsoleRunResult RunConsole(params string[] arguments)
    {
        return RunConsole(arguments, standardInput: null);
    }

    private static ConsoleRunResult RunConsoleWithStandardInput(string standardInput)
    {
        return RunConsole(Array.Empty<string>(), standardInput);
    }

    private static ConsoleRunResult RunConsole(string[] arguments, string? standardInput)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            WorkingDirectory = FindSolutionRoot()
        };

        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(Path.Combine("src", "Iress.ToyRobot.Console", "Iress.ToyRobot.Console.csproj"));
        startInfo.ArgumentList.Add("--");

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start console application.");

        if (standardInput is not null)
        {
            process.StandardInput.WriteLine(standardInput);
        }

        process.StandardInput.Close();

        var standardOutput = process.StandardOutput.ReadToEnd();
        var standardError = process.StandardError.ReadToEnd();

        if (!process.WaitForExit(TimeSpan.FromSeconds(30)))
        {
            process.Kill(entireProcessTree: true);
            throw new TimeoutException("Console application did not exit within 30 seconds.");
        }

        return new ConsoleRunResult(process.ExitCode, standardOutput, standardError);
    }

    private static string FindSolutionRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Iress.ToyRobot.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find solution root.");
    }

    private sealed record ConsoleRunResult(
        int ExitCode,
        string StandardOutput,
        string StandardError);
}
