# Iress Toy Robot Challenge

A C# console application that simulates a toy robot moving on a `5 x 5` tabletop.

[![ci](https://github.com/harvey-zhu-mel/iress-toy-robot-challenge/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/harvey-zhu-mel/iress-toy-robot-challenge/actions/workflows/build-and-test.yml)

## Prerequisites

- .NET SDK `10.0.x`
- Git, if cloning from GitHub

The solution targets `net10.0` and can be built and run on macOS, Windows, or Linux.

## Getting Started

Clone the repository:

```bash
git clone https://github.com/harvey-zhu-mel/iress-toy-robot-challenge.git
cd iress-toy-robot-challenge
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

Run the test suite:

```bash
dotnet test
```

## Run The Application

Run with an input file:

```bash
dotnet run --project ./src/Iress.ToyRobot.Console -- ./testdata/example-a.txt
```

Run with standard input:

```bash
dotnet run --project ./src/Iress.ToyRobot.Console
```

Then enter commands, one per line.

- macOS/Linux: press `Ctrl+D` to finish input.
- Windows: press `Ctrl+Z`, then `Enter` to finish input.

Example input:

```text
PLACE 1,2,EAST
MOVE
MOVE
LEFT
MOVE
REPORT
```

Expected output:

```text
3,3,NORTH
```

## Challenge Rules

The application simulates a toy robot on a square tabletop with dimensions `5 x 5`.

- Valid coordinates are `0..4` for both `X` and `Y`.
- The origin `0,0` is the south-west corner.
- The robot can face `NORTH`, `SOUTH`, `EAST`, or `WEST`.
- The first effective command must be a valid `PLACE X,Y,F`.
- Commands before the first valid `PLACE` are ignored.
- `MOVE` advances the robot by one unit in the direction it is facing.
- `LEFT` and `RIGHT` rotate the robot by 90 degrees without changing its position.
- `REPORT` writes the current `X,Y,F` to standard output.
- Any command that would place or move the robot off the tabletop is ignored.
- Invalid commands are ignored so later valid commands can still be processed.
- No graphical output is required.

Supported commands:

```text
PLACE X,Y,F
MOVE
LEFT
RIGHT
REPORT
```

## Sample Data

Sample command files are included in `testdata`.

| File | Expected output |
| --- | --- |
| `testdata/example-a.txt` | `0,1,NORTH` |
| `testdata/example-b.txt` | `0,0,WEST` |
| `testdata/example-c.txt` | `3,3,NORTH` |

Run all sample files:

```bash
dotnet run --project ./src/Iress.ToyRobot.Console -- ./testdata/example-a.txt
dotnet run --project ./src/Iress.ToyRobot.Console -- ./testdata/example-b.txt
dotnet run --project ./src/Iress.ToyRobot.Console -- ./testdata/example-c.txt
dotnet run --project ./src/Iress.ToyRobot.Console -- ./testdata/edge-cases.txt
```

## Project Structure

```text
src/
  Iress.ToyRobot.Core/       Domain model, command parser, and simulator
  Iress.ToyRobot.Console/    Console application entry point
tests/
  Iress.ToyRobot.Tests/      Unit and integration-style tests
testdata/                    Sample command files
```

## Implementation Notes

- `ToyRobot` owns placement, movement, turning, reporting, and tabletop safety rules.
- `TableTop` defines valid tabletop coordinates.
- `DirectionExtensions` contains direction rotation and report formatting.
- `CommandParser` converts text input into typed command records.
- `RobotSimulator` processes command streams and returns report output.
- The console project stays thin so the core behavior can be tested directly.

The production projects do not use third-party runtime packages. NuGet package dependencies are limited to the test project:

- `xunit`
- `xunit.runner.visualstudio`
- `Microsoft.NET.Test.Sdk`
- `coverlet.collector`

## Input Handling

- Commands are parsed case-insensitively.
- Extra whitespace around commands and `PLACE` arguments is accepted.
- `REPORT` output uses uppercase directions.
- Invalid `PLACE` commands do not clear an existing valid robot state.
- Unsafe `MOVE` commands are ignored, and later valid commands still run.
- `MOVE`, `LEFT`, `RIGHT`, and `REPORT` are ignored until the robot has been placed successfully.
