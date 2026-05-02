using System.Text.RegularExpressions;

namespace Chess.Core.Board;

public readonly partial struct Position
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

    public static bool TryMoveUp(ref Position position)
    {
        if (position.Row == 7) return false;

        position = Create(position.Column, position.Row + 1);
        return true;
    }

    public static bool TryMoveDown(ref Position position)
    {
        if (position.Row == 0) return false;
        
        position = Create(position.Column, position.Row - 1);
        return true;
    }

    public static bool TryMoveLeft(ref Position position)
    {
        if (position.Column == Column.A) return false;

        position = Create(position.Column - 1, position.Row);
        return true;
    }

    public static bool TryMoveRight(ref Position position)
    {
        if (position.Column == Column.H) return false;

        position = Create(position.Column + 1, position.Row);
        return true;
    }

    public static bool TryMoveLeftUp(ref Position position)
    {
        if (position.Column == Column.A || position.Row == 7) return false;

        position = Create(position.Column - 1, position.Row + 1);
        return true;
    }

    public static bool TryMoveLeftDown(ref Position position)
    {
        if (position.Column == Column.A || position.Row == 0) return false;

        position = Create(position.Column - 1, position.Row - 1);
        return true;
    }

    public static bool TryMoveRightUp(ref Position position)
    {
        if (position.Column == Column.H || position.Row == 7) return false;

        position = Create(position.Column + 1, position.Row + 1);
        return true;
    }

    public static bool TryMoveRightDown(ref Position position)
    {
        if (position.Column == Column.H || position.Row == 0) return false;
        
        position = Create(position.Column + 1, position.Row - 1);
        return true;
    }
}