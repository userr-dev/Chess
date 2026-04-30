using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Board;

public class Square(Color color, Column column, int row)
{
    public Color Color { get; } = color;
    public Position Position { get; } = Position.Create(column, row);

    public IPiece? Piece { get; set; }
    
    public bool IsPawnPromotion { get; init; }

    public override string ToString()
    {
        return $"{Position} {Color} {IsPawnPromotion}";
    }
}