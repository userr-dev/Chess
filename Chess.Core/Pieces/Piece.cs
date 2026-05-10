using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public abstract class Piece : IPiece
{
    public Color Color { get; }
    public Position Position { get; set; }

    protected Piece(Color color, Position position)
    {
        Color = color;
        Position = position;
    }
    
    public abstract MoveResult GetAvailableMoves(ChessBoard chessBoard);

    public abstract IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);
    
    public override string ToString()
    {
        return $"{GetType().Name} {Position}";
    }
}