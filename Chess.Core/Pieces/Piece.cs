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
    
    public virtual void Move(ChessBoard chessBoard, Position to)
    {
        chessBoard[Position].Piece = null;
        chessBoard[to].Piece = this;
        Position = to;
    }

    public override string ToString()
    {
        return $"{GetType().Name} {Position}";
    }
}