namespace Chess.Core.Pieces;

public sealed class Bishop : SlidingPiece
{
    private static readonly MoveDirection[] Directions =
        [Position.TryMoveLeftUp, Position.TryMoveLeftDown, Position.TryMoveRightUp, Position.TryMoveRightDown];

    public Bishop(Color color, Position position) : base(color, position)
    {
    }

    public override MoveResult GetAvailableMoves(ChessBoard chessBoard) =>
        GetMovesAlongDirections(chessBoard, Directions);

    internal override IEnumerable<Position> GetAttackedPositions(ChessBoard chessBoard) =>
        GetAttackedPositionsAlongDirections(chessBoard, Directions);

    internal override void FindPinnedPiece(ChessBoard chessBoard, King enemyKing) =>
        FindPinnedPieceAlongDirections(chessBoard, Directions, enemyKing);
}