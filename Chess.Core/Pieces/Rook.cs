using Chess.Core.Board;
using Chess.Core.Pieces.Interfaces;

namespace Chess.Core.Pieces;

public sealed class Rook : SlidingPiece, ICastlingPiece
{
    private static readonly MoveDirection[] Directions =
        [Position.TryMoveUp, Position.TryMoveDown, Position.TryMoveLeft, Position.TryMoveRight];

    public bool CanCastle { get; private set; } = true;
    
    public Rook(Color color, Position position) : base(color, position)
    {
    }

    public override void Move(ChessBoard chessBoard, Position to)
    {
        CanCastle = false;
        
        base.Move(chessBoard, to);
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard) =>
        GetMovesAlongDirections(chessBoard, Directions);

    public override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositionsAlongDirections(chessBoard, Directions);
}