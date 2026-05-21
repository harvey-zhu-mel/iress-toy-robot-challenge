namespace Iress.ToyRobot.Core;

public sealed class TableTop
{
    public const int DefaultWidth = 5;
    public const int DefaultHeight = 5;
    public static readonly TableTop Standard = new(DefaultWidth, DefaultHeight);

    public int Width { get; }
    public int Height { get; }

    public TableTop(int width, int height)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be a positive integer.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be a positive integer.");
        }

        Width = width;
        Height = height;
    }

    public bool Contains(Position position)
    {
        return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
    }
}