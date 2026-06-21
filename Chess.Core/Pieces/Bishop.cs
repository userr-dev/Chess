namespace Chess.Core.Pieces;

public sealed class Bishop : SlidingPiece
{
    private static readonly Direction[] Directions =
        [Direction.LeftUp, Direction.LeftDown, Direction.RightDown, Direction.RightUp];

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