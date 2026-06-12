namespace Chess.Core.Board;

public class Square(Color color, Position position)
{
    public Color Color { get; } = color;
    public Position Position { get; } = position;

    public Piece? Piece { get; internal set; }
    public bool HasPiece => Piece is not null;

    private bool HasPieceOfColor(Color color)
    {
        return HasPiece && Piece!.Color == color;
    }

    public bool HasFriendlyPiece(Color color)
    {
        return HasPieceOfColor(color);
    }

    public bool HasEnemyPiece(Color color)
    {
        return HasPieceOfColor(color.Opposite());
    }
    
    public override string ToString()
    {
        return $"{Piece}";
    }
}