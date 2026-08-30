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
    
    public bool IsPromotionRow(Color pawnColor) => pawnColor == Color.Light ? Row == 7 : Row == 0;

    public (int ColumnOffset, int RowOffset) OffsetFrom(Position to)
    {
        var columnDelta = Column.Delta(to.Column);
        var rowDelta = Row - to.Row;
        return (columnDelta, rowDelta);
    }
    
    public override string ToString()
    {
        return $"{Column.ToNotation()}{Row + 1}";
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

    public static bool IsInDirection(Position from, Position to, Position target)
    {
        if (target == from || target == to) return true;

        var (toFromColumnDelta, toFromRowDelta) = to.OffsetFrom(from);
        var (targetFromColumnDelta, targetFromRowDelta) = target.OffsetFrom(from);

        if (toFromColumnDelta * targetFromRowDelta != toFromRowDelta * targetFromColumnDelta) return false; // Collinear
        
        var dot = toFromColumnDelta * targetFromColumnDelta + toFromRowDelta * targetFromRowDelta; // Scalar Product
        var lengthSqr = toFromColumnDelta * toFromColumnDelta + toFromRowDelta * toFromRowDelta;
        
        return dot >= 0 && dot <= lengthSqr;
    }

    public static Direction GetDirectionFromTo(Position from, Position to)
    {
        var (columnDelta, rowDelta) = to.OffsetFrom(from);
        return new Direction(columnDelta, rowDelta);
    }
    
    public static bool TryMove(ref Position position, int columnOffset, int rowOffset)
    {
        var isColumnShifted = Column.TryShift(position.Column, columnOffset, out var newColumn);
        var newRow = position.Row + rowOffset;

        if (!isColumnShifted || newRow is < 0 or > 7) return false;

        position = Create(newColumn, newRow);
        return true;
    }

    public static bool TryMove(ref Position position, Direction direction)
    {
        return TryMove(ref position, direction.ColumnOffset, direction.RowOffset);
    }
}