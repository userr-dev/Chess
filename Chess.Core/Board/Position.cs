using System.Text.RegularExpressions;

namespace Chess.Core.Board;

public readonly partial record struct Position
{
    private const string PositionFormat = "^[A-Ha-h][1-8]$";
    
    [GeneratedRegex(PositionFormat)]
    private static partial Regex PositionRegex { get; }
    
    public Column Column { get; }
    public int Row { get; }

    private Position(Column column, int row)
    {
        Column = column;
        Row = row;
    }
    
    public override string ToString()
    {
        return $"{Column}{Row + 1}";
    }

    public static Position Create(Column column, int row)
    {
        if (row is < 0 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "Row must be between 0 and 7.");
        }

        return new Position(column, row);
    }
    
    public static Position Parse(string s)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(s);
        
        if (!PositionRegex.IsMatch(s))
        {
            throw new FormatException($"Input '{s}' is not a valid Position. Expected format: column (A-H) followed by row (1-8).");
        }

        var column = Enum.Parse<Column>($"{s[0]}", true);
        var row = s[1] - '0';
        
        return new Position(column, row - 1);
    }

    public static bool TryMove(ref Position position, int columnOffset, int rowOffset)
    {
        var newColumn = (int)position.Column + columnOffset;
        var newRow = position.Row + rowOffset;

        if (newColumn is < 0 or > 7 || newRow is < 0 or > 7) return false;

        position = Create((Column)newColumn, newRow);
        return true;
    }
    
    public static bool TryMoveUp(ref Position position) => TryMove(ref position, 0, 1);
    public static bool TryMoveDown(ref Position position) => TryMove(ref position, 0, -1);
    public static bool TryMoveLeft(ref Position position) => TryMove(ref position, 0, -1);
    public static bool TryMoveRight(ref Position position) => TryMove(ref position, 0, 1);

    public static bool TryMoveLeftUp(ref Position position) => TryMove(ref position, -1, 1);
    public static bool TryMoveLeftDown(ref Position position) => TryMove(ref position, -1, -1);
    public static bool TryMoveRightUp(ref Position position) => TryMove(ref position, 1, 1);
    public static bool TryMoveRightDown(ref Position position) => TryMove(ref position, 1, -1);
}