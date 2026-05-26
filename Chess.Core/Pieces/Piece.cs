using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public abstract class Piece : IPiece
{
    public Color Color { get; }
    public Position Position { get; private set; }
    public MoveDirection[]? AllowedDirections { get; set; }
    public bool IsPinned => AllowedDirections is not null;

    protected Piece(Color color, Position position)
    {
        Color = color;
        Position = position;
    }

    public abstract MoveResult GetAvailableMoves(ChessBoard chessBoard);

    public abstract IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard);
    
    public virtual void Move(ChessBoard chessBoard, Position to)
    {
        chessBoard.MovePiece(this, to);
        Position = to;
    }

    public override string ToString()
    {
        return $"{GetType().Name} {Position}";
    }
}