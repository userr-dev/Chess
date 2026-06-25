namespace Chess.Core.Board;

public readonly record struct Direction(int ColumnOffset, int RowOffset)
{
    public Direction[] GetAxis()
    {
        var oppositeSide = new Direction(ColumnOffset * -1, RowOffset * -1);
        return [this, oppositeSide];
    }

    public static Direction Up => new(0, 1);
    public static Direction Down => new(0, -1);
    public static Direction Left => new(-1, 0);
    public static Direction Right => new(1, 0);

    public static Direction LeftUp => new(-1, 1);
    public static Direction LeftDown => new(-1, -1);
    public static Direction RightUp => new(1, 1);
    public static Direction RightDown => new(1, -1);
}