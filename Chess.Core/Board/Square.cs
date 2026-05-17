using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Board;

public class Square(Color color, Position position)
{
    public Color Color { get; } = color;
    public Position Position { get; } = position;

    public IPiece? Piece { get; set; }
    public bool HasPiece => Piece is not null;

    public bool HasPieceOfColor(Color color)
    {
        return HasPiece && Piece!.Color == color;
    }

    public override string ToString()
    {
        return $"{Position} {Color}";
    }
}